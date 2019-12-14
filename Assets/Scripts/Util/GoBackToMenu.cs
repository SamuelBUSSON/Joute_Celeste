using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UnityTemplateProjects.Util
{
    public class GoBackToMenu : MonoBehaviour
    {
        private void Update()
        {
            if (Gamepad.current.bButton.wasPressedThisFrame)
                SceneManager.LoadScene(0);
        }
    }
}