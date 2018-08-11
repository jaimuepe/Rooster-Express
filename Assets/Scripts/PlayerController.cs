using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Transform _transform;
    private Transform cameraTransform;
    private Rigidbody rb;

    public float speed;

    // Use this for initialization
    void Start()
    {
        _transform = transform;
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        var x = moveHorizontal * Time.deltaTime * speed;
        var z = moveVertical * Time.deltaTime * speed;

        Vector3 cameraFwd = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraFwd);

        Vector3 fwd = Vector3.Cross(cameraRight, Vector3.up);

        rb.MovePosition(transform.position + x * cameraRight + z * fwd);

        if (moveHorizontal != 0.0f || moveVertical != 0.0f)
        {
            Vector3 direction = (x * cameraRight + z * fwd).normalized;
            _transform.forward = direction;
        }
    }
}
