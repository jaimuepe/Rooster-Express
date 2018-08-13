using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFollows : MonoBehaviour {
    
    public readonly Color32 COLOR_GREEN = new Color32(20, 240, 30, 255);
    public readonly Color32 COLOR_RED = new Color32(240, 20, 30, 255);

    public int normalCoinsValue;
    public int hitCoinsValue;
    public int wrongCoinsValue;
    public int burntCoinsValue;

    public string NORMAL_TEXT;
    public string HIT_TEXT;
    public string WRONG_TEXT;
    public readonly string BURNT_TEXT;

    private Transform _playerTransform;
    private TextMeshProUGUI textMesh;

    public TextFollows () {
        NORMAL_TEXT = "+" +normalCoinsValue + " good!";
        HIT_TEXT = "-" +hitCoinsValue + " hit!";
        WRONG_TEXT = "-" +wrongCoinsValue + " wrong!";
        BURNT_TEXT = "-" +burntCoinsValue + " deemn!";
    }

	// Use this for initialization
	void Start () 
    {
        textMesh = GetComponentInParent<TextMeshProUGUI>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
    void Update () 
    {
        transform.localEulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + 1.3f, _playerTransform.position.z);
    }

    public void showMessage(string text, Color32 color) {
        textMesh.text = text;
        textMesh.color = color;
        textMesh.autoSizeTextContainer = true;
    }
}
