using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public float Width = 1f;
    public float Length = 1f;
    public float Height = 1f;
    private bool editorMode = true;
    public Texture2D texture;

    private MeshCollider mc;
    public ProceduralMesh proceduralMesh;

    // Use this for initialization
    void Start()
    {
        proceduralMesh = new BoxProceduralMesh();
        proceduralMesh.Parameters["Width"] = Width;
        proceduralMesh.Parameters["Length"] = Length;
        proceduralMesh.Parameters["Height"] = Height;
        proceduralMesh.Generate(2);
        //proceduralMesh.FlipNormals(); Flip normals for editor, not for testing

        gameObject.AddComponent<MeshFilter>().mesh = proceduralMesh.Mesh;

        mc = gameObject.GetComponent<MeshCollider>();
        mc.sharedMesh = GetComponent<MeshFilter>().mesh;
        //mc.convex = true; Off for editor, on for testing
        mc.convex = true;

        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Standard"));
        mr.material.EnableKeyword("_MainTex");
        mr.material.SetTexture("_MainTex", texture);

        /*GameObject go = new GameObject("Bounds");
        go.transform.parent = this.transform;
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().size = proceduralMesh.Mesh.bounds.size;
        go.layer = LayerMask.NameToLayer("Ignore Raycast");
        */

        MeshDeformer md = gameObject.AddComponent<MeshDeformer>();

        //Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        //rb.mass = 50;//gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        mc.sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    void EnableEditorMode()
    {

    }

    void EnableSimulationMode()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (editorMode) EditorFixedUpdate();
    }

    void EditorFixedUpdate()
    {
        if (Width != proceduralMesh.Parameters["Width"] || Height != proceduralMesh.Parameters["Height"] || Length != proceduralMesh.Parameters["Length"])
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

        if (!proceduralMesh.Flipped)
        {
            proceduralMesh.FlipNormals();
            //gameObject.GetComponent<MeshFilter>().mesh = proceduralMesh.Mesh;
        }
    }
}
