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
                if(caja.boxWasThrown) {
                    gameManager.itemsThrownCorrect += 1;
                }
                gameManager.SuccessfulDelivery(caja.code == "E");
                FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                
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
                        FindObjectOfType<LightsManager>().TurnRed(screenLight);
                        break;
                    default:
                        break;
                }
            }
            Destroy(other.gameObject);
        }
    }
}