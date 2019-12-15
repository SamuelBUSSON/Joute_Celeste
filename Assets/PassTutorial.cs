using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UnityTemplateProjects.Util
{

    public class PassTutorial : MonoBehaviour
    {

        public GameObject[] gameObjects;
        private int i = 0;


        // Update is called once per frame
        void Update()
        {
            if (Gamepad.current.aButton.wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
            {
                i++;

                if (i == gameObjects.Length)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    gameObjects[i].SetActive(true);
                }
              
            }
        }
    }
}