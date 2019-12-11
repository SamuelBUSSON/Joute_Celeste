using System;
using UnityEngine;

namespace UnityTemplateProjects.AI
{
    public class AIController : MonoBehaviour
    {
        public AIState state;

        private void Start()
        {
            state = new AIStateLookForProj();
            
            state.OnStart();
        }

        private void Update()
        {
            state.OnUpdate();
        }
    }
}