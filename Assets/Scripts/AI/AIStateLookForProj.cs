using UnityEngine;

namespace UnityTemplateProjects.AI
{
    public class AIStateLookForProj : AIState
    {
        public float sightRange;

        private Vector3 projPosition;

        public float cooldownSearch;
        private float cooldownSearchCurrent;
        
        public override void OnStart()
        {
            projPosition = Vector2.zero;

            cooldownSearchCurrent = cooldownSearch;
            
            LookForProjectile();
        }

        private void LookForProjectile()
        {
            print("looking for proj");
            print(controller.transform.position);
            var hits = Physics2D.CircleCastAll(controller.transform.position, sightRange, Vector2.one);

            for (int i = 0; i < hits.Length; i++)
            {
                var proj = hits[i].transform.GetComponent<Projectile>();
                if (proj)
                {
                    projPosition = proj.transform.position;
                }
            }
        }

        public override void OnUpdate()
        {
            cooldownSearchCurrent -= Time.deltaTime;
            
            if (projPosition != Vector3.zero)
            {
                controller.transform.position = Vector3.MoveTowards(controller.transform.position, projPosition, 0.1f * Time.deltaTime);

                if ((controller.transform.position - projPosition).magnitude <= .5f)
                {
                    projPosition = Vector3.zero;
                    print("yep, found that");
                }
            }
            else if(cooldownSearchCurrent <= 0)
            {
                cooldownSearchCurrent = cooldownSearch;
                
                LookForProjectile();
            }
        }
    }
}