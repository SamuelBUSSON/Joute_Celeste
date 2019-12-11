using UnityEngine;

namespace UnityTemplateProjects.AI
{

    public abstract class AIState : MonoBehaviour
    {
        public AIController controller;
        
        public abstract void OnStart();
        public abstract void OnUpdate();
    }
}