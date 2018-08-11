using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItems : MonoBehaviour {

    private bool carrying = false;
    private Collider caja;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(carrying == true) {
            caja.GetComponent<Rigidbody>().isKinematic = false;
            caja.transform.position = transform.position + new Vector3(0, 0, 0.5f);
        }
	}

private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Caja"))
        {
            caja = other;
            if (Input.GetButtonUp("Jump") && carrying == true)
            {
                other.GetComponent<Rigidbody>().isKinematic = true;
                carrying = false;
            } else if (Input.GetButtonUp("Jump") && carrying == false)
            {
                carrying = true;
            }
        }
    }
}
