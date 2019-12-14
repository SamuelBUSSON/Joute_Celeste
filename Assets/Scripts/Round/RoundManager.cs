using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float timerInSecond = 10;
    
    //[NonSerialized]
    public float timer = 0.0f;

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
