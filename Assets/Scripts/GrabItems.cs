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

    public AudioClip grabClip;

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

    public bool TryPickUpBox()
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
            boxes[0].gameObject.GetComponent<Caja>().pickedUpFor1stTime = true;

            AudioUtils.PlayClip2D(grabClip, 1.0f);
            return true;
        }
        return false;
    }

    public bool CarryingItem { get { return boxTransform != null; } }
    public Transform BoxTransform { get { return boxTransform; } }

    public void DropBox(float forceOfDrop, Vector3 throwPosition)
    {
        boxTransform.gameObject.GetComponent<Caja>().positionThrow = throwPosition;
        boxTransform.GetComponent<Rigidbody>().isKinematic = false;
        boxTransform.GetComponent<Rigidbody>().AddForce((transform.TransformDirection(Vector3.up +Vector3.forward) * forceOfDrop), ForceMode.Impulse);
        boxTransform.GetComponent<Rigidbody>().AddTorque(transform.TransformDirection(Vector3.right) * forceOfDrop / 50f, ForceMode.Impulse);
        boxTransform = null;
    }

    private void PickUpBox(GameObject box)
    {
        boxTransform = box.transform;
        boxTransform.GetComponent<Rigidbody>().isKinematic = true;
        BoxCollider boxC = boxTransform.GetComponent<BoxCollider>();
        distance = 0.05f + boxC.size.z * boxTransform.localScale.z * 0.5f;
    }
}
