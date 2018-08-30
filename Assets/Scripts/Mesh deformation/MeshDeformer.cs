using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {
    private Vector3[] originalVertices, displacedVertices;
    private Mesh deformingMesh;

    // Use this for initialization
    void Start () {
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];

		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
	}
	
	// Update is called once per frame
	void Update () {

    }

    void UpdateMesh()
    {
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            float mass = collision.collider.GetComponent<Rigidbody>().mass / GetComponent<Rigidbody>().mass;

            AddDeformingForce(contact.point, contact.normal, collision.relativeVelocity.magnitude * 0.01f);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        UpdateMesh();
    }

    struct Vector3Ref
    {
        public int index;
        public Vector3 vector;

        public Vector3Ref(int index, Vector3 vector)
        {
            this.index = index;
            this.vector = vector;
        }
    }

    public void AddDeformingForce(Vector3 point, Vector3 normal, float force) {
        List<Vector3Ref> sortedVertices = new List<Vector3Ref>();
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            sortedVertices.Add(new Vector3Ref(i, displacedVertices[i]));
        }

        sortedVertices.Sort((v1, v2) => {
            float d1 = (transform.TransformPoint(v1.vector) - point).sqrMagnitude;
            float d2 = (transform.TransformPoint(v2.vector) - point).sqrMagnitude;

            return d1.CompareTo(d2);
        });

        for (int i = 0; i < 3; i++)
        {
            Vector3Ref r = sortedVertices[i];

            r.vector = sortedVertices[i].vector + normal * force;

            sortedVertices[i] = r;
        }

        foreach (Vector3Ref vectorRef in sortedVertices)
        {
            displacedVertices[vectorRef.index] = vectorRef.vector;
        }

        //displacedVertices = sortedVertices.ToArray();

        /*if (smallestIndex >= 0 && ((displacedVertices[smallestIndex] - originalVertices[smallestIndex]).sqrMagnitude < 0.002f))
        {

            displacedVertices[smallestIndex] = displacedVertices[smallestIndex] + normal * force;
        }*/
    }
}
