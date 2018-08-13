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
            Caja caja = other.gameObject.GetComponent<Caja>();
            string state = new string[] { "A", "B", "C", "D", "E" }[index];

            if (state == caja.code)
            {
                gameManager.SuccessfulDelivery();
                gameManager.incrementPoints(caja);
                if(caja.boxWasThrown) {
                    gameManager.boxesThrownCorrect += 1;
                }
                gameManager.SuccessfulDelivery(objectCode == "E");
                FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                gameManager.incrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
                
                switch (state)
                {
                    case "A":
                        FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                        break;
                    case "B":
                        FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                        break;
                    case "C":
                        FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                        break;
                    case "D":
                        FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                        break;
                    case "E":
                        FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                gameManager.UnsucessfulDelivery(state == "E");
                FindObjectOfType<LightsManager>().TurnRed(screenLight);
                gameManager.decrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
                
                switch (state)
                {
                    case "A":
                        FindObjectOfType<LightsManager>().TurnRed(screenLight);
                        break;
                    case "B":
                        FindObjectOfType<LightsManager>().TurnRed(screenLight);
                        break;
                    case "C":
                        FindObjectOfType<LightsManager>().TurnRed(screenLight);
                        break;
                    case "D":
                        FindObjectOfType<LightsManager>().TurnRed(screenLight);
                        break;
                    case "E":
                        FindObjectOfType<LightsManager>().TurnRed(4);
                        gameManager.burntBoxPoints(caja);
                        FindObjectOfType<LightsManager>().TurnRed(screenLight);
                        break;
                    default:
                        gameManager.decrementPoints(caja);
                        break;
                }
            }
            Destroy(other.gameObject);
        }
    }
}