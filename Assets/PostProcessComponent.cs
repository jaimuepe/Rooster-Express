using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessComponent : MonoBehaviour
{
    PostProcessingProfile m_Profile;

    private void OnEnable()
    {
        PostProcessingBehaviour behaviour = GetComponent<PostProcessingBehaviour>();
        if (behaviour.profile == null)
        {
            enabled = false;
            return; 
        }

        m_Profile = Instantiate(behaviour.profile);
        behaviour.profile = m_Profile;
    }

    public void EnableDepthOfField()
    {
        m_Profile.depthOfField.enabled = true;
    }

    public void DisableDepthOfField()
    {
        m_Profile.depthOfField.enabled = false;
    }
}
