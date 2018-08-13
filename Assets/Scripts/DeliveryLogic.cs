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
                FindObjectOfType<LightsManager>().TurnGreen(screenLight);
                gameManager.incrementPoints((float)System.Math.Round(other.GetComponent<Caja>().points, 2));
				
                switch (state)
                {
                    case "A":
                        FindObjectOfType<LightsManager>().TurnGreen(0);
                        break;
                    case "B":
                        FindObjectOfType<LightsManager>().TurnGreen(1);
                        break;
                    case "C":
                        FindObjectOfType<LightsManager>().TurnGreen(2);
                        break;
                    case "D":
                        FindObjectOfType<LightsManager>().TurnGreen(3);
                        break;
                    case "E":
                        FindObjectOfType<LightsManager>().TurnGreen(4);
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
                        FindObjectOfType<LightsManager>().TurnRed(0);
                        break;
                    case "B":
                        FindObjectOfType<LightsManager>().TurnRed(1);
                        break;
                    case "C":
                        FindObjectOfType<LightsManager>().TurnRed(2);
                        break;
                    case "D":
                        FindObjectOfType<LightsManager>().TurnRed(3);
                        break;
                    case "E":
                        FindObjectOfType<LightsManager>().TurnRed(4);
                        break;
                    default:
                        break;
                }
            }
            Destroy(other.gameObject);
        }
    }
}
