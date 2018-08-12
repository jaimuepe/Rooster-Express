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
        this.playerPoints += points;
        pointsUI.text = "Points: " + playerPoints.ToString();
    }

}
