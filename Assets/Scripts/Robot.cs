using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public Texture2D texture;

    ProceduralMesh proceduralMesh;
    private MeshCollider mc;

    public float Width = 1f;
    public float Length = 1f;
    public float Height = 1f;

    public bool flipped = false;

    // Use this for initialization
    void Start()
    {
        proceduralMesh = new BoxProceduralMesh();
        proceduralMesh.Parameters["Width"] = Width;
        proceduralMesh.Parameters["Length"] = Length;
        proceduralMesh.Parameters["Height"] = Height;

        proceduralMesh.Generate(2);
        //proceduralMesh.FlipNormals();

        gameObject.AddComponent<MeshFilter>().mesh = proceduralMesh.Mesh;
        
        gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_MainTex");
        gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture);
          
        mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = GetComponent<MeshFilter>().mesh;
        mc.convex = true;

        /*GameObject go = new GameObject("Bounds");
        go.transform.parent = this.transform;
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().size = proceduralMesh.Mesh.bounds.size;
        go.layer = LayerMask.NameToLayer("Ignore Raycast");
        */

        gameObject.AddComponent<MeshDeformer>();
        gameObject.GetComponent<MeshDeformer>().springForce = 0;

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 50;//gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        mc.sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Width != proceduralMesh.Parameters["Width"] || Height != proceduralMesh.Parameters["Height"] || Length != proceduralMesh.Parameters["Length"])
        {
            proceduralMesh.Parameters["Width"] = Width;
            proceduralMesh.Parameters["Length"] = Length;
            proceduralMesh.Parameters["Height"] = Height;

            proceduralMesh.Generate(2);
            //proceduralMesh.FlipNormals();

            gameObject.GetComponent<MeshFilter>().mesh = proceduralMesh.Mesh;
            Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.AddComponent<BoxCollider>();
        }

        if (flipped != proceduralMesh.Flipped)
        {
            proceduralMesh.FlipNormals();
            gameObject.AddComponent<MeshCollider>();
        }*/
    }
}
