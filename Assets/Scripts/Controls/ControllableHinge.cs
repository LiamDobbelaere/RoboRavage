using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableHinge : MonoBehaviour {
    private HingeJoint joint;

    public Vector2 horizontalVelocities;
    public Vector2 verticalVelocities;

    // Use this for initialization
    void Start () {
        joint = GetComponent<HingeJoint>();
	}
	
	// Update is called once per frame
	void Update () {
        JointMotor motor = joint.motor;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0.1f)
        {
            motor.targetVelocity = horizontalVelocities.y;
        }
        else if (horizontal < -0.1f)
        {
            motor.targetVelocity = horizontalVelocities.x;
        }
        else if (vertical > 0.1f)
        {
            motor.targetVelocity = verticalVelocities.y;
        }
        else if (vertical < -0.1f)
        {
            motor.targetVelocity = verticalVelocities.x;
        }
        else
        {
            motor.targetVelocity = 0f;
        }

        joint.motor = motor;
    }
}
