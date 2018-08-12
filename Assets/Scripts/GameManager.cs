using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private float playerPoints;
    Canvas canvas;

    public Text pointsUI;

	// Use this for initialization
	void Start () {
        playerPoints = 0;
        canvas = FindObjectOfType<Canvas>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void incrementPoints(float points) {
        this.playerPoints += 1;
        pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja correcta");
    }

    public void decrementPoints(float points) {
        this.playerPoints -= 1;
        pointsUI.text = "Points: " + playerPoints.ToString();
        Debug.Log("Caja incorrecta");
    }

}
