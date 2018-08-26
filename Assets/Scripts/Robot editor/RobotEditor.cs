using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEditor : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float maxDistance = 5.0f;
    public float minDistance = 1f;
    public float xSpeed = 125.0f;
    public float zSpeed = 125.0f;

    bool rightclicked;
    bool first;
    float x = 0.0f;
    float y = 0.0f;
    Vector3 panDeltaPosition;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.z;

        Quaternion rotation = Quaternion.Euler(0.1f * xSpeed * distance * 0.02f, 0f * xSpeed * distance * 0.02f, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
        transform.rotation = rotation;
        transform.position = position;

    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Clamp(distance + -Input.GetAxis("Mouse ScrollWheel"), minDistance, maxDistance);

        if (target && (Input.GetMouseButton(1) || Input.GetMouseButton(2) || (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.0f)))
        {
            //target.transform.LookAt(transform);

            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * xSpeed * 0.02f;
            }
            else if (Input.GetMouseButton(2))
            {
                panDeltaPosition= new Vector3(
                    panDeltaPosition.x + Input.GetAxis("Mouse X") * zSpeed * 0.02f,
                    panDeltaPosition.y + Input.GetAxis("Mouse Y") * zSpeed * 0.02f,
                    panDeltaPosition.z);
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.localPosition;
            transform.rotation = rotation;
            transform.position = position + (panDeltaPosition.x * transform.right) + (panDeltaPosition.y * transform.up);
        }
    }
}
