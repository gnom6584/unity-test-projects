using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float ZoomRadius;

    [SerializeField] float lookSpeedH = 2f;

    [SerializeField] float lookSpeedV = 2f;

    [SerializeField] float zoomSpeed = 2f;

    [SerializeField] float dragSpeed = 3f;   

    float _yaw = 0f;

    float _pitch = 0f;

    private void Start()
    {
        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _yaw += lookSpeedH * Input.GetAxis("Mouse X");
            _pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
        }

        if (Input.GetMouseButton(2))
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);

        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);

        // transform.position = new Vector3(Mathf.Clamp(transform.position.x, LimitMinX, LimitMaxX), Mathf.Clamp(transform.position.y, LimitMinY, LimitMaxY), Mathf.Clamp(transform.position.z, LimitMinZ, LimitMaxZ));
        var distance = transform.position.magnitude;
        if(distance > ZoomRadius)
            distance = ZoomRadius;

        transform.position = transform.position.normalized * distance;
        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, 0.0f), transform.position.z);
    }
}
