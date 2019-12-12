using System;
using System.Collections;
using DG.Tweening;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.VFX;

namespace AI
{
    
    public enum States
    {
        Init,
        LookForProj,
        ChargeProj,
        Attack
    }
    
    
    public class AIController : MonoBehaviour
    {
        public States startingState;
        StateMachine<States> fsm;

        [Header("Looking for projectile")] 
        public float sightRange;
        public Projectile inHand;
        public bool drawSight;

        [Header("Attacking")] public float launchStrength;
        public float knockbackForce;

        private PlayerHealth health;

        private void Awake()
        {
            fsm = StateMachine<States>.Initialize(this);
            fsm.ChangeState(startingState);

            health = GetComponent<PlayerHealth>();
        }

        private void OnDrawGizmos()
        {
            if (drawSight)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.position, sightRange);
            }
            
        }

        void Init_Enter()
        {
            StartCoroutine(WaitABit());
        }

        private IEnumerator WaitABit()
        {
            yield return new WaitForEndOfFrame();
            fsm.ChangeState(States.LookForProj);
        }

        void LookForProj_Enter()
        {
           CheckForProj();
        }

        void LookForProj_Update()
        {
            CheckForProj();
        }

        private void CheckForProj()
        {
            print("Looking for proj");
            var hits = Physics2D.CircleCastAll(transform.position, sightRange, Vector2.zero);

            if (hits.Length >= 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Projectile proj = hits[i].transform.GetComponent<Projectile>();

                    if (proj)
                    {
                        inHand = proj;
                        
                        fsm.ChangeState(States.Attack);
                    }
                }
            }
            else
            {
                print("shit");
            }
        }

        void Attack_Enter()
        {
            print("Attack !");
            Projectile projectile = inHand.GetComponent<Projectile>();
            projectile.isLaunched = true;
            projectile.tag = "Untagged";
            //projectile.currentDamage *= damageMultiplier;
            projectile.playerIndex = GameManager.Instance.player2.playerIndex;


            AkSoundEngine.SetSwitch("Choix_Astres",
                projectile.type == EProjectileType.PLANET ? "Planete" :
                projectile.type == EProjectileType.STAR ? "Etoile" : "Comete", gameObject);
            AkSoundEngine.PostEvent("Play_Player_Fire", gameObject);

            Vector3 heading = (GameManager.Instance.player1.transform.position - transform.position).normalized;

            inHand.transform.position = transform.position + (heading * 2);
            
            inHand.GetComponent<Rigidbody2D>().velocity = projectile.speed * launchStrength * heading;

            VisualEffect fx = inHand.GetComponent<VisualEffect>();
            fx.SetBool("SpawnRate", false);

            inHand.GetComponentsInChildren<VisualEffect>()[1].enabled = true;

            Vector2 v1 = transform.position;
            Vector2 v2 = heading;

            transform.DOMove(v1 - v2 * knockbackForce, 0.05f);

            inHand = null;
        }
    }
}