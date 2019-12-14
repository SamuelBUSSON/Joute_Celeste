using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Player;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

public class ObjectHandler : MonoBehaviour
{

    public float distance = 1.0f;

    public float launchStrength = 2.0f;
    [NonSerialized] public float damageMultiplier = 1f;

    public float knockbackForce = 2.0f;

    public Transform enemyPos;

    public bool isHoldingStar = false;
    public bool isStarChargLv1 = false;
    public bool isHoldingObject = false;

    private Transform handledObject;
    private float angle = 0.0f;
    private Vector3 displaceAngleVector;

    private float coolDownTimer = 0.0f;

    private PlayerInput input;

    private bool autoAim = false;

    private Displacement playerMovement;
    private PlayerZone playerZone;
    private PlayerHealth playerHealth;

    private PlayerController controller;

    private uint playerCharge;
    private uint playerHeal;

    private Animator animator;
    private static readonly int Release = Animator.StringToHash("Release");
    private static readonly int Hold = Animator.StringToHash("Hold");

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        input.actions.Enable();

        input.currentActionMap["Fire"].canceled += context => OnFire(context);

        input.currentActionMap["Aim"].performed += context => OnAim(context);
        input.currentActionMap["Aim"].canceled += context => OnAutoAim(context);

        input.currentActionMap["HoldLv1"].performed += OnHoldLv1;
        input.currentActionMap["HoldLv2"].performed += OnHoldLv2;

        input.currentActionMap["CaugthLv1"].performed += OnDrainStar;

        enemyPos = transform;
        controller = GetComponent<PlayerController>();

        playerHealth = GetComponent<PlayerHealth>();

        animator = GetComponent<Animator>();

    }


    private void OnHoldLv1(InputAction.CallbackContext obj)
    {
        if (handledObject)
        {
            Projectile proj = handledObject.GetComponent<Projectile>();

            proj.GetComponentInChildren<VisualEffect>().SendEvent("OnCast");

            proj.GetComponent<Rigidbody2D>().AddTorque(0.1f, ForceMode2D.Impulse);

            AkSoundEngine.PostEvent("Play_Player_Charge", gameObject);

            proj?.GetComponent<SpriteRenderer>().material.DOFloat((proj.GetComponent<SpriteRenderer>().material.GetFloat("_PowerColor") * 2f), "_PowerColor", 0.3f);

            proj.SetThresholdLevel(0);

            if (proj.type == EProjectileType.STAR)
            {
                isStarChargLv1 = true;
            }
        }
    }


    private void OnHoldLv2(InputAction.CallbackContext obj)
    {
        if (handledObject)
        {
            Projectile proj = handledObject.GetComponent<Projectile>();

            AkSoundEngine.PostEvent("Play_Player_Charge", gameObject);

            VisualEffect fx = proj.GetComponentInChildren<VisualEffect>();


            fx.SetFloat("Radius", proj.size + 0.5f);
            fx.SendEvent("OnCast");

            proj.GetComponentInChildren<VisualEffect>().SendEvent("OnCast");

            proj?.GetComponent<SpriteRenderer>().material.DOFloat((proj.GetComponent<SpriteRenderer>().material.GetFloat("_PowerColor") * 1.5f), "_PowerColor", 0.3f);

            proj.SetThresholdLevel(1);

            if (proj.type == EProjectileType.STAR)
            {
                isHoldingStar = true;
            }
            else
            {
                isHoldingObject = true;
            }

            playerCharge = AkSoundEngine.PostEvent("Play_Player_Charge_Loop", gameObject);
        }
    }

    private void OnDrainStar(InputAction.CallbackContext obj)
    {
        Projectile proj = handledObject?.GetComponent<Projectile>();
        if (proj.type == EProjectileType.STAR && !isStarChargLv1)
        {
            GameObject healObject = Instantiate(proj.healFx, proj.transform.position, Quaternion.identity);
            DrainStar(proj, healObject);

            playerHeal = AkSoundEngine.PostEvent("Play_Player_Charge_Loop", gameObject);
        }
    }


    private void DrainStar(Projectile proj, GameObject healObject)
    {
        handledObject = null;

        VisualEffect healFx = healObject.GetComponent<VisualEffect>();
        healFx.SendEvent("OnHeal");

        playerHealth.GiveHealth(playerHealth.maxHealth / 3);
        proj.transform.DOScale(0, 2f).OnUpdate(() => UpdateHeal(healFx, proj)).OnComplete(() => DestroyStar(healFx, proj));
    }

    private void UpdateHeal(VisualEffect healFx, Projectile proj)
    {
        if (proj.transform.localScale.x <= 0.5f)
        {
            healFx.SetFloat("Rate", 0);
        }
        healFx.SetVector3("PlayerPosition", transform.position - proj.transform.position);
    }

    private void DestroyStar(VisualEffect healFx, Projectile proj)
    {
        healFx.SetFloat("Radius", 0.1f);
        healFx.SetFloat("AttractionForce", 5000);
        healFx.SetFloat("StickForce", 50000);
        healFx.SetVector3("PlayerPosition", transform.position - proj.transform.position);

        StartCoroutine(UpdatePosFx(healFx, proj.transform.position, proj));
        AkSoundEngine.StopPlayingID(playerHeal);

    }

    private IEnumerator UpdatePosFx(VisualEffect healFx, Vector3 position, Projectile proj)
    {
        for (int i = 0; i < 20; i++)
        {
            healFx.SetVector3("PlayerPosition", transform.position - position);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(proj.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        displaceAngleVector = new Vector3();

        playerMovement = GetComponent<Displacement>();

        playerZone = GetComponentInChildren<PlayerZone>();
    }


    // Update is called once per frame
    void Update()
    {
        if (handledObject)
        {
            if (autoAim)
            {
                Aim(Vector2.zero, true);
            }
            handledObject.transform.position = transform.position + displaceAngleVector * handledObject.GetComponent<Projectile>().size; ;
        }


    }

    private void FixedUpdate()
    {
        if (isHoldingStar)
        {
            CameraManager.Instance.Shake(2.0f, 2.0f, Time.fixedTime);

            if (input.user.pairedDevices[0] is Gamepad pad)
            {
                CameraManager.Instance.Vibrate(0.2f, 0.0f, Time.fixedTime, pad);
            }
        }
        if (isHoldingObject)
        {
            CameraManager.Instance.Shake(0.1f, 0.1f, Time.fixedTime);

            if (input.user.pairedDevices[0] is Gamepad pad)
            {
                CameraManager.Instance.Vibrate(0f, 0.05f, Time.fixedTime, pad);
            }
        }
    }

    private void LateUpdate()
    {
        if (handledObject)
        {
            handledObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        if (!playerMovement.IsDashing())
        {
            Fire();
        }
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        autoAim = false;
        if (handledObject)
        {
            Aim(obj.ReadValue<Vector2>(), false);
        }
    }

    private void OnAutoAim(InputAction.CallbackContext obj)
    {
        autoAim = true;
    }

    private void Fire()
    {
        if (handledObject)
        {
            animator.SetTrigger(Release);
            animator.SetBool(Hold, false);
        }
        else
        {
            Transform objectToLaunch = playerZone.GetNearestObjectInZone(true);

            if (objectToLaunch && objectToLaunch.GetComponent<Projectile>().isChopable)
            {
                handledObject = objectToLaunch;

                if (handledObject)
                {
                    SetObjectHandled(handledObject);
                    handledObject.gameObject.layer = 10;
                    animator.SetTrigger(Release);
                    animator.SetBool(Hold, false);
                }
            }
        }
    }

    /// <summary>
    /// CALLED BY THE ANIMATOR
    /// </summary>
    public void CallCaught()
    {
        playerZone.CaughtObject();
    }

    public void FireObject()
    {
        FireObject(false);
    }

    private void FireObject(bool aimingToPlayer = false)
    {

        Projectile proj = handledObject.GetComponent<Projectile>();

        bool canLaunch = true;

        if (proj.type == EProjectileType.STAR && proj.thresholdIndex != 1)
        {
            canLaunch = false;
        }

        if (canLaunch)
        {
            CameraManager.Instance.Shake(5.0f, 5.0f, 0.1f);
            //TODO: check findgamepad id

            if (input.user.pairedDevices[0] is Gamepad pad)
            {
                CameraManager.Instance.Vibrate(0.8f, 0.0f, 0.1f, pad);
            }

            handledObject.SetParent(null);

            Projectile projectile = handledObject.GetComponent<Projectile>();
            projectile.isLaunched = true;
            projectile.tag = "Untagged";
            projectile.currentDamage *= damageMultiplier;
            projectile.playerIndex = controller.playerIndex;

            handledObject.gameObject.layer = 10;

            switch (projectile.type)
            {
                case EProjectileType.ASTEROID:
                    AkSoundEngine.PostEvent("Play_Move_Comete", gameObject);
                    break;
                case EProjectileType.PLANET:
                    AkSoundEngine.PostEvent("Play_Move_Planete", gameObject);
                    break;
                case EProjectileType.STAR:
                    AkSoundEngine.PostEvent("Play_Move_Etoile", gameObject);
                    break;
                default:
                    break;
            }

            AkSoundEngine.SetSwitch("Choix_Astres", projectile.type == EProjectileType.PLANET ? "Planete" : projectile.type == EProjectileType.STAR ? "Etoile" : "Comete", gameObject);
            AkSoundEngine.PostEvent("Play_Player_Fire", gameObject);
            AkSoundEngine.StopPlayingID(playerCharge);

            Vector3 heading = aimingToPlayer ? -(handledObject.transform.position - enemyPos.position).normalized : (handledObject.transform.position - transform.position).normalized;

            handledObject.GetComponent<Rigidbody2D>().velocity = projectile.speed * launchStrength * heading;

            VisualEffect fx = handledObject.GetComponent<VisualEffect>();
            fx.SetBool("SpawnRate", false);

            handledObject.GetComponentsInChildren<VisualEffect>()[1].enabled = true;

            Vector2 v1 = transform.position;
            Vector2 v2 = heading;

            transform.DOMove(v1 - v2 * knockbackForce, 0.05f);

            handledObject = null;

            isHoldingStar = false;
            isHoldingObject = false;
            isStarChargLv1 = false;
        }
    }

    private void Aim(Vector2 aimDirection, bool autoAim)
    {
        Vector3 heading = enemyPos ? enemyPos.position - transform.position : Vector3.zero;

        float x = aimDirection.x;
        float y = aimDirection.y;

        angle = autoAim ? Mathf.Atan2(heading.normalized.x, heading.normalized.y) : Mathf.Atan2(x, y);

        displaceAngleVector.x = distance * Mathf.Sin(angle);
        displaceAngleVector.y = distance * Mathf.Cos(angle);

    }

    public void SetObjectHandled(Transform objectToThrow)
    {
        objectToThrow?.GetComponent<SpriteRenderer>().material.SetInt("_HighLigth", 0);

        handledObject = objectToThrow;

        handledObject.GetComponent<Projectile>().isChopable = false;

        handledObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        handledObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        handledObject.GetComponent<Rigidbody2D>().inertia = 0;

        handledObject.gameObject.layer = 13;

        handledObject.SetParent(transform);

        Aim(Vector2.zero, true);
    }

    public Transform GetObjectHandled()
    {
        return handledObject;
    }

    public void SetEnemyPos(Transform t)
    {
        enemyPos = t;
    }


}