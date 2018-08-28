using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NocollideBody : MonoBehaviour {
    public Collider other;

	// Use this for initialization
	void Start () {
        Physics.IgnoreCollision(GetComponent<Collider>(), other);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
