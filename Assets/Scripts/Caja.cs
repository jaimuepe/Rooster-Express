using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    public bool fragile;
    public float points;
    public string code;

    public float maxValueForHit;

    public Vector3 positionThrow;
    public float maxDistanceDone;
    public float speedCollision;
    public float maxSpeedCollision;
    public float defaultThrowDistance;
    public bool boxWasThrown;
    public bool pickedUpFor1stTime;

    GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (pickedUpFor1stTime)
        {
            speedCollision = GetComponent<Rigidbody>().velocity.magnitude;
            if (speedCollision > maxSpeedCollision)
            {
                maxSpeedCollision = speedCollision;
            }
            if(fragile && speedCollision > maxValueForHit) {
                gameManager.fragileBoxesHits += 1;
            }
            float distance = Vector3.Distance(positionThrow, transform.position);
            if (distance > maxDistanceDone)
            {
                maxDistanceDone = distance;
            }
            if (gameManager.maximumThrowDistance < maxDistanceDone) {
                gameManager.maximumThrowDistance = maxDistanceDone;
            }
            if(distance > defaultThrowDistance) {
                gameManager.itemsThrown += 1;
                boxWasThrown = true;
            } else {
                boxWasThrown = false;
            }
            distance = 0;
        }
        pickedUpFor1stTime = false;
    }
}
