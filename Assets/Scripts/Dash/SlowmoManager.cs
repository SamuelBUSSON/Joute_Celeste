using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmoManager : MonoBehaviour
{
    public static SlowmoManager instance;


    public float frequency = 0.03f;
    public float destoryTime = 0.3f;

    private bool isSlowMo = false;

    // Set the singleton
    void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else
        {
            //If instance already exists and it's not this:
            if (instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    public bool GetSlowMo()
    {
        return isSlowMo;
    }

    public void SetSlowMo(bool new_slowMo)
    {
        isSlowMo = new_slowMo;
    }
   
}
