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
    public float speedMultiplier;
    public float dragDivisor;

    public float maxForce;
    public float maxTimerThrow;

    float deltaTimeForce;

    private float initialDrag;

    public float smoothDamp;
    Vector3 dampVelocity = Vector3.zero;

    public Transform detailCameraContainerTransform;
    private Camera detailCamera;

    Camera mainCamera;

    bool detailView;

    public float detailZoomSpeed;

    public float minDetailDistance;
    public float maxDetailDistance;
    private float detailDistance;

    public float mouseRotationSpeed;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = Camera.main;
        deltaTimeForce = 0f;

        _transform = transform;
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        initialDrag = rb.drag;
        grabber = GetComponentInChildren<GrabItems>();

        lastDirection = _transform.forward;

        detailCamera = detailCameraContainerTransform.GetComponentInChildren<Camera>();
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
                else
                {
                    float mouseScrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
                    if (mouseScrollWheelDelta != 0.0f)
                    {
                        detailDistance = Mathf.Clamp(detailDistance - mouseScrollWheelDelta * detailZoomSpeed, minDetailDistance, maxDetailDistance);
                        detailCamera.orthographicSize = detailDistance;
                    }
                }
            }
            else
            {
                if (Input.GetButton("Fire1"))
                {
                    deltaTimeForce += Time.deltaTime;
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    float forceOfDrop = 0f;
                    forceOfDrop = maxForce * Mathf.Clamp01(deltaTimeForce / maxTimerThrow);
                    Debug.Log("Time: " + deltaTimeForce + ", force: " + forceOfDrop);
                    grabber.DropBox(forceOfDrop);
                    forceOfDrop = 0.0f;
                    deltaTimeForce = 0f;
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    EnterDetailView();
                }
            }
        }
        else
        {
            if (Input.GetButtonUp("Fire1"))
            {
                grabber.TryPickUpBox();
            }
        }
    }

    private void PlayerMovement()
    {
        float localSpeed;
        if (Input.GetButton("Fire3"))
        {
            Debug.Log("La velocidad ha aumentado");
            localSpeed = speed * speedMultiplier;
            if (rb.velocity.magnitude > 0.1f && rb.drag > 0.5f)
            {
                rb.drag -= 0.1f;
            }
        }
        else
        {
            localSpeed = speed;
            if (rb.drag < initialDrag)
            {
                rb.drag += 0.3f;
            }
        }
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraFwd = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraFwd);

        Vector3 fwd = Vector3.Cross(cameraRight, Vector3.up);
        Vector3 direction = (moveHorizontal * cameraRight + moveVertical * fwd).normalized;

        Vector3 velocityVector = direction * Time.deltaTime * localSpeed;

        rb.velocity += velocityVector;

        if (moveHorizontal != 0.0f || moveVertical != 0.0f)
        {
            lastDirection = direction;
        }
        else
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            if (Mathf.Abs(mouseDelta.x) > .1f)
            {
                float deltaAngle = mouseDelta.x;

                // TODO no queda bien, revisar
                Vector3 lookVector = (mouseDelta.x * cameraRight + mouseDelta.y * fwd).normalized;
                // lastDirection = Vector3.RotateTowards(lastDirection, lookVector, .5f, .5f);
            }
        }

        _transform.forward = Vector3.Slerp(_transform.forward, lastDirection, smoothDamp);
    }

    Vector3 lastDirection;

    private void EnterDetailView()
    {
        detailDistance = 1.0f;
        detailCamera.orthographicSize = detailDistance;

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
        detailDistance = 1.0f;

        detailView = false;
        PostProcessComponent ppComponent = mainCamera.GetComponent<PostProcessComponent>();
        ppComponent.DisableDepthOfField();

        Transform boxT = grabber.BoxTransform;
        boxT.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Boxes"));

        detailCameraContainerTransform.gameObject.SetActive(false);
        detailCameraContainerTransform.SetParent(null, false);
    }
}
