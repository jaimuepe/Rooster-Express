using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    Transform _transform;

    void Start()
    {
        _transform = transform;
    }

    public float speed;
    public float damping;

    private void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Vector3 deltaRotation = Vector3.zero;
        if (mouseDelta.x != 0.0f || mouseDelta.y != 0.0f)
        {
            if (mouseDelta.x != 0.0f)
            {
                deltaRotation.x = mouseDelta.x;
            }
            if (mouseDelta.y != 0.0f)
            {
                deltaRotation.y = mouseDelta.y;
            }
        } else
        {
            mouseDelta = new Vector2(Input.GetAxis("Controller Mouse X"), Input.GetAxis("Controller Mouse Y"));
            if (mouseDelta.x != 0.0f)
            {
                deltaRotation.x = 3 * mouseDelta.x;
            }
            if (mouseDelta.y != 0.0f)
            {
                deltaRotation.y = 3 * mouseDelta.y;
            }
        }

        _transform.Rotate(-deltaRotation.y, deltaRotation.x, 0.0f, Space.Self);
    }
}
