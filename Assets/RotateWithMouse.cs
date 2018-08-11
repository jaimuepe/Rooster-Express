using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{

    Transform _transform;
    Vector2 lastMousePosition;

    Vector3 rotation;

    void Start()
    {
        _transform = transform;
        rotation = transform.eulerAngles;
        lastMousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public float speed;
    public float damping;

    private void Update()
    {
        Vector2 currentMousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Debug.Log(mouseDelta);

        Vector3 deltaRotation = Vector3.zero;
        if (mouseDelta.x != 0.0f)
        {
            deltaRotation.y = mouseDelta.x;
        }
        if (mouseDelta.y != 0.0f)
        {
            deltaRotation.x = -mouseDelta.y;
        }

        rotation += deltaRotation * speed * Time.deltaTime;

        Quaternion desiredRotQ = Quaternion.Euler(rotation);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, desiredRotQ, Time.deltaTime * damping);

        lastMousePosition = currentMousePosition;
    }
}
