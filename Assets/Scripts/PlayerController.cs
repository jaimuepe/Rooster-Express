using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Transform _transform;
    private Transform cameraTransform;
    private Rigidbody rb;

    private GrabItems grabber;

    public float speed;

    public float smoothDamp;
    Vector3 dampVelocity = Vector3.zero;

    public Transform detailCameraContainerTransform;

    Camera mainCamera;

    bool detailView;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = Camera.main;

        _transform = transform;
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        grabber = GetComponentInChildren<GrabItems>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!detailView)
        {
            PlayerMovement();
        }

        if (grabber.CarryingItem)
        {
            if (detailView)
            {
                if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
                {
                    ExitDetailView();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    grabber.DropBox();
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    EnterDetailView();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                grabber.TryPickUpBox();
            }
        }
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
        Vector3 direction = (x * cameraRight + z * fwd).normalized;

        Vector3 moveVector = direction * Time.deltaTime * speed;

        rb.MovePosition(transform.position + moveVector);

        if (moveHorizontal != 0.0f || moveVertical != 0.0f)
        {
            _transform.forward = Vector3.Slerp(_transform.forward, direction, smoothDamp);
        }
    }

    private void EnterDetailView()
    {
        detailView = true;

        PostProcessComponent ppComponent = mainCamera.GetComponent<PostProcessComponent>();
        ppComponent.EnableDepthOfField();

        Transform boxT = grabber.BoxTransform;
        boxT.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Detail"));

        detailCameraContainerTransform.gameObject.SetActive(true);
        detailCameraContainerTransform.SetParent(boxT, false);
    }

    private void ExitDetailView()
    {
        detailView = false;

        PostProcessComponent ppComponent = mainCamera.GetComponent<PostProcessComponent>();
        ppComponent.DisableDepthOfField();

        Transform boxT = grabber.BoxTransform;
        boxT.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Boxes"));
        boxT.gameObject.layer = 0;

        detailCameraContainerTransform.gameObject.SetActive(false);
        detailCameraContainerTransform.SetParent(null, false);
    }
}
