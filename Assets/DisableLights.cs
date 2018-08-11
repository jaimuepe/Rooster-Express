using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DisableLights : MonoBehaviour
{
    public Light[] lights;

    private void OnPreCull()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = false;
        }
    }

    private void OnPostRender()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
        }
    }
}
