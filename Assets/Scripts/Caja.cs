using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour {
    
    public float points;
    public string code;

    public Vector3 positionThrow;
    public float speedCollision;
    public float maxSpeedCollision;
    public bool pickedUpFor1stTime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(pickedUpFor1stTime) {
            speedCollision = GetComponent<Rigidbody>().velocity.magnitude;
            if(speedCollision > maxSpeedCollision) {
                maxSpeedCollision = speedCollision;
            }
        }
    }

}
