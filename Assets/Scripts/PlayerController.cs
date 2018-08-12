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

    public GrabItems grabber;

    public float speed;
    public float speedMultiplier;
    public float dragDivisor;

    public float maxForce;
    public float maxTimerThrow;

    float deltaTimeForce;

    private float initialDrag;

    public float minimumDrag;
    public float dragDecrement;
    public float dragIncrement;

    public float smoothDamp;
    Vector3 dampVelocity = Vector3.zero;

    public Transform detailCameraContainerTransform;
    private Camera detailCamera;

    Camera mainCamera;

    bool detailView;

    public float detailMovementSpeed;
    public float detailZoomSpeed;

    public float minDetailDistance;
    public float maxDetailDistance;
    private float detailDistance;

    public float mouseRotationSpeed;

    // TUTORIAL
    public bool playerGrabbedFirstBox;

    Animator animator;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponent<Animator>();

        mainCamera = Camera.main;
        deltaTimeForce = 0f;

        _transform = transform;
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        initialDrag = rb.drag;

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
                    if (Input.GetButton("Fire3"))
                    {
                        Vector3 displacement = detailCamera.transform.localPosition;

                        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                        if (mouseDelta.x != 0.0f)
                        {
                            displacement -= mouseDelta.x * detailMovementSpeed * Vector3.right;
                        }
                        if (mouseDelta.y != 0.0f)
                        {
                            displacement -= mouseDelta.y * detailMovementSpeed * Vector3.up;
                        }

                        displacement.x = Mathf.Clamp(displacement.x, -1.0f, 1.0f);
                        displacement.y = Mathf.Clamp(displacement.y, -1.0f, 1.0f);

                        detailCamera.transform.localPosition = displacement;
                    }
                    else
                    {
                        float mouseScrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");

                        if (mouseScrollWheelDelta != 0.0f)
                        {
                            detailDistance = Mathf.Clamp(detailDistance + mouseScrollWheelDelta * detailZoomSpeed, minDetailDistance, maxDetailDistance);
                            detailCamera.transform.localPosition =
                                new Vector3(detailCamera.transform.localPosition.x,
                                detailCamera.transform.localPosition.y,
                                detailDistance);
                        }

                        float moveHorizontal = Input.GetAxis("Horizontal");
                        float moveVertical = Input.GetAxis("Vertical");

                        if (moveHorizontal != 0.0f || moveVertical != 0.0f)
                        {
                            Vector3 displacement = detailCamera.transform.localPosition;

                            Vector2 movementDelta = new Vector2(moveHorizontal, moveVertical);

                            if (movementDelta.x != 0.0f)
                            {
                                displacement += movementDelta.x * detailMovementSpeed * Vector3.right;
                            }
                            if (movementDelta.y != 0.0f)
                            {
                                displacement += movementDelta.y * detailMovementSpeed * Vector3.up;
                            }

                            displacement.x = Mathf.Clamp(displacement.x, -1.0f, 1.0f);
                            displacement.y = Mathf.Clamp(displacement.y, -1.0f, 1.0f);

                            detailCamera.transform.localPosition = displacement;
                        }
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
                if (grabber.TryPickUpBox())
                {
                    playerGrabbedFirstBox = true;
                }
            }
        }

        animator.SetFloat("speed", rb.velocity.sqrMagnitude);
    }

    private void PlayerMovement()
    {
        float localSpeed;
        if (Input.GetButton("Fire3"))
        {
            localSpeed = speed * speedMultiplier;
            if (rb.velocity.magnitude > 0.1f && rb.drag > minimumDrag)
            {
                rb.drag -= dragDecrement;
            }
        }
        else
        {
            localSpeed = speed;
            if (rb.drag < initialDrag)
            {
                rb.drag += dragIncrement;
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
                lastDirection = Vector3.RotateTowards(lastDirection, lookVector, .1f, .1f);
            }
        }

        _transform.forward = Vector3.Slerp(_transform.forward, lastDirection, smoothDamp);
    }

    Vector3 lastDirection;

    private void EnterDetailView()
    {
        detailDistance = -1.0f;
        detailCamera.transform.localPosition = detailDistance * Vector3.forward;

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
        detailDistance = -1.0f;

        detailView = false;
        PostProcessComponent ppComponent = mainCamera.GetComponent<PostProcessComponent>();
        ppComponent.DisableDepthOfField();

        Transform boxT = grabber.BoxTransform;
        boxT.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Boxes"));

        detailCameraContainerTransform.gameObject.SetActive(false);
        detailCameraContainerTransform.SetParent(null, false);
    }
}
