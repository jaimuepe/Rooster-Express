using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLogic : MonoBehaviour
{
    GameManager gameManager;

    public string state;

    public Renderer screen;

    public Material matDistrict;
    public Material matPriority;

    bool displayingDistrict = true;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void District()
    {
        screen.material = matDistrict;
    }

    public void SwapDisplay()
    {
        StartCoroutine(Coroutines.Chain(
            Coroutines.BlinkFastFast(screen.material, "_Color", 4.0f, 0.5f, 0.01f),
            Coroutines.Wrap(() =>
            {
                if (displayingDistrict)
                {
                    screen.material = matPriority;
                }
                else
                {
                    screen.material = matDistrict;
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

            if (state == objectCode)
            {
                gameManager.SuccessfulDelivery();
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
