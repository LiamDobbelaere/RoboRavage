using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEditor : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float maxDistance = 5.0f;
    public float minDistance = 1f;
    public float rotateSpeed = 125.0f;
    public float panSpeed = 8f;

    bool rightclicked;
    bool first;
    float x = 0.0f;
    float y = 0.0f;
    Vector3 panDeltaPosition;

    Quaternion targetRotation;
    Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.z;

        Quaternion rotation = Quaternion.Euler(0.1f * rotateSpeed * distance * 0.02f, 0f * rotateSpeed * distance * 0.02f, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
        targetRotation = rotation;
        targetPosition = position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        distance = Mathf.Clamp(distance + -Input.GetAxis("Mouse ScrollWheel"), minDistance, maxDistance);

        if (target && (Input.GetMouseButton(1) || Input.GetMouseButton(2) || (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.0f)))
        {
            //target.transform.LookAt(transform);

            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * rotateSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * rotateSpeed * 0.02f;
            }
            else if (Input.GetMouseButton(2))
            {
                panDeltaPosition = new Vector3(
                    panDeltaPosition.x + Input.GetAxis("Mouse X") * panSpeed * 0.02f,
                    panDeltaPosition.y + Input.GetAxis("Mouse Y") * panSpeed * 0.02f,
                    panDeltaPosition.z);
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.localPosition;
            targetRotation = rotation;
            targetPosition = position + (panDeltaPosition.x * transform.right) + (panDeltaPosition.y * transform.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 20f);
    }
}
