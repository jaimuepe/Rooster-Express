using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScreen : MonoBehaviour
{
    public Material relaxed;
    public Material what;
    public Material angry;
    public Material[] talking;

    Renderer renderer;

    int current = 0;
    int currentTalkingIdx;

    float talkingTimer;
    float whatTimer;
    float angryTimer;

    public float talkingDuration;
    public float whatDuration;
    public float angryDuration;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (current == 3)
        {
            // talk
            talkingTimer += Time.deltaTime;
            if (talkingTimer >= talkingDuration)
            {
                currentTalkingIdx = (currentTalkingIdx + 1) % talking.Length;
                renderer.material = talking[currentTalkingIdx];
                talkingTimer = 0.0f;
            }
        }
        else if (current == 0)
        {
            // idle
        }
        else if (current == 1)
        {
            // what
            whatTimer += Time.deltaTime;
            if (whatTimer >= whatDuration)
            {
                whatTimer = 0.0f;
                renderer.material = relaxed;
                current = 0;
            }
        }
        else if (current == 2)
        {
            // angry
            angryTimer += Time.deltaTime;
            if (angryTimer > angryDuration)
            {
                angryTimer = 0.0f;
                renderer.material = relaxed;
                current = 0;
            }
        }
    }

    public void Talk()
    {
        if (current == 3)
        {
            return;
        }

        whatTimer = 0.0f;
        angryTimer = 0.0f;
        current = 3;
        renderer.material = talking[0];
    }

    public void Relaxed()
    {
        if (current == 0)
        {
            return;
        }
        talkingTimer = 0.0f;
        whatTimer = 0.0f;
        angryTimer = 0.0f;

        renderer.material = relaxed;
        current = 0;
    }

    public void What()
    {
        if (current == 1)
        {
            whatTimer -= whatDuration;
            return;
        }
        else if (current == 2)
        {
            angryTimer -= whatDuration * 0.5f;
            return;
        }

        angryTimer = 0.0f;

        renderer.material = what;
        current = 1;
    }

    public void Angry()
    {
        if (current == 2)
        {
            angryTimer -= angryDuration;
            return;
        }
        whatTimer = 0.0f;

        renderer.material = angry;
        current = 2;
    }
}
