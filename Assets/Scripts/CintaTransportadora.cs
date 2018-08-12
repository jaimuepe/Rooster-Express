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
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject caja = other.gameObject;
        if(caja.CompareTag(TAG_BOX)) {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = transform.TransformDirection(Vector3.up) * force;
        }
    }
}
