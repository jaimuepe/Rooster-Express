using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLogic : MonoBehaviour {

    GameManager gameManager;

    public string state;

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
            Destroy(other.gameObject);
            if(state.Equals(other.gameObject.GetComponent<Caja>().code)) {
                gameManager.incrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
            } else {
                gameManager.decrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
            }
        }
    }
}
