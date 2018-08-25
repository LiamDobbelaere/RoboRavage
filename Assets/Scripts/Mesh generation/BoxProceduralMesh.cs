using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxProceduralMesh : ProceduralMesh {    
    protected override Mesh GenerateMesh()
    {
        float w = Parameters["Width"];
        float l = Parameters["Length"];
        float h = Parameters["Height"];

        Vector3[] vertices = new Vector3[] {                         
            new Vector3(-w, -h, -l),     //3  B-BL (Bottom face)
            new Vector3(w, -h, -l),      //2  B-BR (Bottom face)
            new Vector3(w, -h, l),       //1  B-TR (Bottom face)                       
            new Vector3(-w, -h, l),      //0  B-TL (Bottom face

            new Vector3(-w, h, l),       //4  T-TL (Top face)
            new Vector3(w, h, l),        //5  T-TR (Top face)
            new Vector3(w, h, -l),       //6  T-BR (Top face)
            new Vector3(-w, h, -l),      //7  T-BL (Top face)
            
            new Vector3(-w, h, l),       //8  T-TL (Left face)
            new Vector3(-w, h, -l),      //9 T-BL (Left face)
            new Vector3(-w, -h, -l),     //10 B-BL (Left face)
            new Vector3(-w, -h, l),      //11  B-TL (Left face)
            
            new Vector3(w, h, -l),       //12 T-BR (Right face)
            new Vector3(w, h, l),        //13 T-TR (Right face)
            new Vector3(w, -h, l),       //14 B-TR (Right face) 
            new Vector3(w, -h, -l),      //15 B-BR (Right face)
                                   
            new Vector3(-w, h, -l),      //16 T-BL (Front face)
            new Vector3(w, h, -l),       //17 T-BR (Front face)
            new Vector3(w, -h, -l),      //18 B-BR (Front face)
            new Vector3(-w, -h, -l),     //19 B-BL (Front face)

            new Vector3(w, h, l),        //20 T-TR (Back face)
            new Vector3(-w, h, l),       //21 T-TL (Back face)
            new Vector3(-w, -h, l),      //22 B-TL (Back face)
            new Vector3(w, -h, l)        //23 B-TR (Back face)
        };

        int[] triangles = new int[] {
            0, 1, 2,
            2, 3, 0,
            
            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };

        Vector2[] uv = new Vector2[] {
            new Vector2(0, l),
            new Vector2(w, l),
            new Vector2(w, 0),
            new Vector2(0, 0),

            new Vector2(0, l),
            new Vector2(w, l),
            new Vector2(w, 0),
            new Vector2(0, 0),

            new Vector2(0, h),
            new Vector2(l, h),
            new Vector2(l, 0),
            new Vector2(0, 0),

            new Vector2(0, h),
            new Vector2(l, h),
            new Vector2(l, 0),
            new Vector2(0, 0),

            new Vector2(0, h),
            new Vector2(w, h),
            new Vector2(w, 0),
            new Vector2(0, 0),

            new Vector2(0, h),
            new Vector2(w, h),
            new Vector2(w, 0),
            new Vector2(0, 0)
        };

        Mesh newMesh = new Mesh();
        newMesh.name = "BoxProceduralMesh";

        newMesh.vertices = vertices;
        newMesh.uv = uv;
        newMesh.triangles = triangles;

        return newMesh;
    }
}
