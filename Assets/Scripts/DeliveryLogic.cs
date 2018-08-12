using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLogic : MonoBehaviour {

    GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Caja")) {
            other.gameObject.SetActive(false);
            gameManager.incrementPoints(Random.Range(1,10));
        }
    }
}
