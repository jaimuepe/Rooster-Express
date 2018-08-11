using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    Transform _transform;

    void Start()
    {
        _transform = transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public float speed;
    public float damping;

    private void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Vector3 deltaRotation = Vector3.zero;
        if (mouseDelta.x != 0.0f)
        {
            deltaRotation.x = mouseDelta.x;
        }
        if (mouseDelta.y != 0.0f)
        {
            deltaRotation.y = mouseDelta.y;
        }

        _transform.Rotate(-deltaRotation.y, deltaRotation.x, 0.0f, Space.Self);
    }
}
