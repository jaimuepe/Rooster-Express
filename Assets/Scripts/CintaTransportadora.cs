using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CintaTransportadora : MonoBehaviour
{
    private const string TAG_BOX = "Caja";

    public float force;

    private void OnCollisionStay(Collision collision)
    {
        GameObject caja = collision.gameObject;
        if (caja.CompareTag(TAG_BOX))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            // rb.velocity = transform.TransformDirection(Vector3.up) * force;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                rb.AddForceAtPosition(transform.TransformDirection(Vector3.up) * force / collision.contacts.Length, collision.contacts[i].point, ForceMode.VelocityChange);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject caja = other.gameObject;
        if (caja.CompareTag(TAG_BOX))
        {
            // Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            // rb.velocity = transform.TransformDirection(Vector3.up) * force;
        }
    }
}
