using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Caja")) {
            other.gameObject.SetActive(false);
        }
    }
}
