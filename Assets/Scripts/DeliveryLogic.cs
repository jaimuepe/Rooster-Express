using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLogic : MonoBehaviour
{
    GameManager gameManager;

    public int index;

    public Renderer screen;

    public Material[] matDistrict;
    public Material[] matPriority;

    bool displayingDistrict = true;

    public Light screenLight;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void District(int index)
    {
        this.index = index;
        displayingDistrict = true;
        screen.material = matDistrict[index];
    }

    public void SwapDisplay(int index)
    {
        StartCoroutine(Coroutines.Chain(
            Coroutines.BlinkFastFast(screen.material, "_Color", 4.0f, 0.5f, 0.01f),
            Coroutines.Wrap(() =>
            {
                this.index = index;
                if (displayingDistrict)
                {
                    screen.material = matPriority[index];
                }
                else
                {
                    screen.material = matDistrict[index];
                }
                displayingDistrict = !displayingDistrict;
            }
            )));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Caja"))
        {
            string objectCode = other.gameObject.GetComponent<Caja>().code;
            string state = new string[] { "A", "B", "C", "D", "E" }[index];

            if (state == objectCode)
            {
                gameManager.SuccessfulDelivery();
                // gameManager.incrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
                FindObjectOfType<LightsManager>().TurnGreen(screenLight);
            }
            else
            {
                gameManager.UnsucessfulDelivery(state == "E");
                // gameManager.decrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
                FindObjectOfType<LightsManager>().TurnRed(screenLight);
            }
            Destroy(other.gameObject);
        }
    }
}
