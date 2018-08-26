using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {
	public float springForce = 20f;
	public float damping = 5f;

    private float uniformScale = 1f;
    private Mesh deformingMesh;
    private Vector3[] originalVertices, displacedVertices;
    private Vector3[] vertexVelocities;

    // Use this for initialization
    void Start () {
		uniformScale = transform.localScale.x;

		deformingMesh = GetComponent<MeshFilter> ().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		vertexVelocities = new Vector3[originalVertices.Length];

		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices [i] = originalVertices [i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex (i);
		}

        //GetComponent<MeshCollider>().sharedMesh = deformingMesh;
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();
	}

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            float mass = collision.collider.GetComponent<Rigidbody>().mass / GetComponent<Rigidbody>().mass;

            AddDeformingForce(contact.point, contact.normal, collision.relativeVelocity.sqrMagnitude * mass);
        }
        if (collision.relativeVelocity.magnitude > 2)
        {

        }
    }

	public void AddDeformingForce(Vector3 point, Vector3 normal, float force) {
		point = transform.InverseTransformPoint (point);
		for (int i = 0; i < displacedVertices.Length; i++) {
			AddForceToVertex (i, point, normal, force);
		}
		//Debug.DrawLine (Camera.main.transform.position, point);
	}

    void AddForceToVertex(int i, Vector3 point, Vector3 normal, float force) {
		Vector3 pointToVertex = displacedVertices [i] - point;
		pointToVertex *= uniformScale;

		float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;

		vertexVelocities[i] += normal * velocity;

		UpdateVertex (i);
	}

	void UpdateVertex(int i) {
		Vector3 velocity = vertexVelocities [i];
		Vector3 displacement = displacedVertices [i] - originalVertices [i];
		displacement *= uniformScale;
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - damping * Time.deltaTime;
		vertexVelocities [i] = velocity;
		displacedVertices [i] += velocity * (Time.deltaTime / uniformScale);
	}
}
