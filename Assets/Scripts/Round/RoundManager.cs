using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    public float timerInSecond = 10;
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timerInSecond)
        {
            GameManager.Instance.TimeUp();
        }
    }
}
