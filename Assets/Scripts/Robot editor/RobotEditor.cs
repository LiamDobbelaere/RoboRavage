using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEditor : MonoBehaviour
{
    /* Camera Controls */
    public Transform target;
    public float distance = 5.0f;
    public float maxDistance = 5.0f;
    public float minDistance = 1f;
    public float rotateSpeed = 125.0f;
    public float panSpeed = 8f;

    private float x = 0.0f;
    private float y = 0.0f;
    private Vector3 panDeltaPosition;
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    /* Components */
    public GameObject[] motors;
    private RobotComponent currentComponent;
    private int currentAttachmentPoint;

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

        currentComponent = Instantiate(motors[0]).GetComponent<RobotComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraPosition();
        UpdateComponentPosition();
    }

    void UpdateComponentPosition()
    {
        if (Input.GetButtonUp("Jump"))
        {
            currentAttachmentPoint++;
            if (currentAttachmentPoint >= currentComponent.attachmentPoints.Count) currentAttachmentPoint = 0;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Vector3 attachmentPosition = currentComponent.attachmentPoints[currentAttachmentPoint].transform.position - currentComponent.transform.position;   

            currentComponent.transform.position = hit.point - attachmentPosition;
            currentComponent.transform.rotation = currentComponent.attachmentPoints[currentAttachmentPoint].transform.localRotation;
        }
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
