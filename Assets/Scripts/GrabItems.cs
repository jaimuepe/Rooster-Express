using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItems : MonoBehaviour
{
    public float distance;

    private Transform boxTransform;
    private Transform _transform;
    private BoxCollider detectionCollider;

    public LayerMask pickUpMask;

    private void Start()
    {
        _transform = transform;
        detectionCollider = GetComponent<BoxCollider>();
    }

    Collider[] boxes = new Collider[3];

    void Update()
    {
        if (CarryingItem)
        {
            boxTransform.position = transform.position + distance * transform.TransformDirection(Vector3.forward);
            boxTransform.rotation = transform.rotation;

        }
    }

    public void TryPickUpBox()
    {
        int results = Physics.OverlapBoxNonAlloc(
            _transform.position + _transform.TransformDirection(detectionCollider.center),
            detectionCollider.size * 0.5f,
            boxes,
            _transform.rotation,
            pickUpMask);

        if (results > 0)
        {
            PickUpBox(boxes[0].gameObject);
        }
    }

    public bool CarryingItem { get { return boxTransform != null; } }
    public Transform BoxTransform { get { return boxTransform; } }

    public void DropBox()
    {
        boxTransform.GetComponent<Rigidbody>().isKinematic = false;
        boxTransform = null;
    }

    private void PickUpBox(GameObject box)
    {
        boxTransform = box.transform;
        boxTransform.GetComponent<Rigidbody>().isKinematic = true;
        BoxCollider boxC = boxTransform.GetComponent<BoxCollider>();
        distance = boxC.size.z * boxTransform.localScale.z * 0.5f;
    }
}
