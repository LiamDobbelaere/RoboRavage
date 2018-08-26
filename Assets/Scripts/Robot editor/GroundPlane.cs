using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlane : MonoBehaviour {
    private Robot robot;

	// Use this for initialization
	void Start () {
        robot = GameObject.Find("Robot").GetComponent<Robot>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0, -robot.proceduralMesh.Parameters["Height"] - 0.001f, 0);
	}
}
