using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public Transform camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
        playerMovement();
	}

    private void playerMovement() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        var x = moveHorizontal * Time.deltaTime * 3.0f;
        var z = moveVertical * Time.deltaTime * 3.0f;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, camera.localEulerAngles.y, transform.localEulerAngles.z);

        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);

        Vector3 movement = new Vector3(camera.eulerAngles.y * x, 0.0f, camera.eulerAngles.y * z);
        transform.rotation = Quaternion.LookRotation(movement);
    }
}
