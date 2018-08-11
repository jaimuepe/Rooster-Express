using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItems : MonoBehaviour
{

    private bool carrying = false;
    private Collider caja;
    public float distance;

    // Update is called once per frame
    void Update()
    {
        if (carrying)
        {
            caja.GetComponent<Rigidbody>().isKinematic = true;
            caja.transform.position = transform.position + distance * transform.TransformDirection(Vector3.forward);
            caja.transform.forward = transform.forward;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Caja"))
        {
            caja = other;
            if (Input.GetButtonUp("Jump") && carrying == true)
            {
                other.GetComponent<Rigidbody>().isKinematic = false;
                carrying = false;
            }
            else if (Input.GetButtonUp("Jump") && carrying == false)
            {
                carrying = true;
            }
        }
    }
}
