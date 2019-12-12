using System;
using System.Collections;
using DG.Tweening;
using MonsterLove.StateMachine;
using UnityEngine;
using UnityEngine.VFX;

namespace AI
{
    
    public enum ActionStates
    {
        Init,
        LookForProj,
        ChargeProj,
        Attack
    }

    public enum AttentionStates
    {
        Idle,
        Dodge
    }
    
    
    public class AIController : MonoBehaviour
    {
        public ActionStates startingActionState;
        StateMachine<ActionStates> actionFsm;
        StateMachine<AttentionStates> attentionFsm;

        [Header("Looking for projectile")] 
        public float sightRange;
        public Projectile inHand;
        public bool drawSight;

        [Header("Attacking")] public float launchStrength;
        public float knockbackForce;

        private PlayerHealth health;
        private Displacement displacement;

        //used for dodging detection
        private float radius;

        private void Awake()
        {
            actionFsm = StateMachine<ActionStates>.Initialize(this);
            attentionFsm = StateMachine<AttentionStates>.Initialize(this);
            
            attentionFsm.ChangeState(AttentionStates.Dodge);
            actionFsm.ChangeState(startingActionState);

            health = GetComponent<PlayerHealth>();
            displacement = GetComponent<Displacement>();
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
            actionFsm.ChangeState(ActionStates.LookForProj);
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

                    //TODO: enlève le !
                    if (proj && !proj.isChopable)
                    {
                        inHand = proj;
                        inHand.isChopable = false;
                        
                        actionFsm.ChangeState(ActionStates.Attack);
                    }
                }
            }
            else
            {
                print("shit");
            }
        }

        void Dodge_Update()
        {
            print("Dodging");
            var hits = Physics2D.CircleCastAll(transform.position, sightRange * 3f, Vector2.zero);

            if (hits.Length >= 0)
            {
                bool found = false;
                for (int i = 0; i < hits.Length && !found; i++)
                {
                    Vector3 dir = hits[i].rigidbody.velocity.normalized;
                    if (sphereHit(dir, dir * 50))
                    {
                        Vector3 dodgingDir = Vector3.Cross(dir, transform.position);
                        
                        displacement.SetMovement(dodgingDir);
                        displacement.Dash();
                    }
                }
            }
        }
        
        private  bool sphereHit(Vector3 va, Vector3 vb)
        {
            Vector3 direction = va - transform.position;
            float a = Vector3.Dot(vb, vb);
            float b = 2.0f * Vector3.Dot(direction, vb);
            float c = Vector3.Dot(direction, direction) - radius * radius;

            float discriminant = b * b - 4 * a * c;

            if (discriminant > 0)
            {
                return true;
            }

            return false;
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
            
            attentionFsm.ChangeState(AttentionStates.Dodge);
        }
    }
}