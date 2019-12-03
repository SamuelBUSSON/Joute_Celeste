using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTemplateProjects.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput input;

        public int playerIndex;
        private void Awake()
        {
            playerIndex = GameManager.Instance.playerIndex;
        }
    }
}