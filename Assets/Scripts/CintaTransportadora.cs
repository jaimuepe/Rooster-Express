using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CintaTransportadora : MonoBehaviour {

    private const string TAG_BOX = "Caja";

    public float force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        GameObject caja = other.gameObject;
        if (caja.CompareTag(TAG_BOX))
        {
            Debug.Log("Empujamos la caja");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 vector = rb.velocity;
            rb.velocity = vector * 0.1f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject caja = other.gameObject;
        if(caja.CompareTag(TAG_BOX)) {
            Debug.Log("Empujamos la caja");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if(rb.rotation.Equals(new Vector3(rb.rotation.x, rb.rotation.y, 0))) {
                rb.freezeRotation = true;
            }
            rb.velocity = new Vector3(force, 0, 0);
        }
    }
}
