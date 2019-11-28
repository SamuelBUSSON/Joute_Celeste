using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerZone : MonoBehaviour
{
    [Header("Aim Triangle")]
    public bool freeLook = false;
    public Transform triangleAim;
    public int slicesCount = 8;

    [Header("Zone Catch")]
    public float distanceToCatch = 1.0f; 

    private Transform player;
    private ObjectHandler playerObjectHandler;

    private List<Transform> objectInZone;
    private List<Transform> objectToSlowOnDash;
    private Transform nearestElement;

    private PlayerInput input;

    private bool isAiming = false;
    private bool canCatch = true;
    

    private void Awake()
    {
        input = GetComponentInParent<PlayerInput>();

        input.actions.Enable();

        input.currentActionMap["CaughtObject"].performed += context => OnCaughtObject(context);

        input.currentActionMap["Aim"].performed += context => OnAim(context);
        input.currentActionMap["Aim"].canceled += context => OnAimCanceled(context);
    }



    // Start is called before the first frame update
    void Start()
    {
        objectInZone = new List<Transform>();
        objectToSlowOnDash = new List<Transform>();

        player = GetComponentInParent<Displacement>().transform;
        playerObjectHandler = player.GetComponent<ObjectHandler>();        
    }

    // Update is called once per frame
    void Update()
    {
         GetNearestElement();         

        if(nearestElement && Vector2.Distance(nearestElement.position, transform.position) <= distanceToCatch)
        {
            CaughtObject(nearestElement);
        }        
    }

    private void OnCaughtObject(InputAction.CallbackContext obj)
    {
        if (isAiming)
        {
            Transform nearestElem = GetComponentInChildren<PlayerZoneAim>().GetNearestObjectInZone();
            if (nearestElem)
            {
                CaughtObject(nearestElem);
            }
        }
        else
        {
            CaughtObject(nearestElement);
        }
    }

    public void CaughtObject(Transform element)
    {        
        if (!playerObjectHandler.GetObjectHandled() && objectInZone.Count > 0 && canCatch)
        {
            canCatch = false;
            ChangeNearestElementColor(false);
            Vector3 heading = element.transform.position - transform.position;

            AimCanceled();            

            element.DOMove(transform.position + heading.normalized, 0.1f).OnComplete(() => CaughtEffect(element));            
        }
    }

    private void CaughtEffect(Transform element)
    {
        canCatch = true;
        playerObjectHandler.SetObjectHandled(element);
        objectInZone.Remove(element);
        nearestElement = element == nearestElement ? null : nearestElement ;
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        if (!playerObjectHandler.GetObjectHandled())
        {
            isAiming = true;
            Aim(obj.ReadValue<Vector2>());
        }
        else
        {
            AimCanceled();
        }
    }

    public void Aim(Vector2 v)
    {
        triangleAim.GetComponentInChildren<SpriteRenderer>().enabled = true;

        int angle = Mathf.RoundToInt( Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);        

        if (!freeLook)
        {
            if(angle > -160 && angle < 160)
            {
                if (angle % slicesCount != 0)
                {
                    angle -= angle % slicesCount;
                }
            }
            else
            {
                angle = -180;
            }
        }

        triangleAim.transform.DORotate(new Vector3(0, 0, angle), 0.1f);
    }

    public void OnAimCanceled(InputAction.CallbackContext obj)
    {
        AimCanceled();
       
    }

    private void AimCanceled()
    {
        isAiming = false;
        triangleAim.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && other.transform != playerObjectHandler.GetObjectHandled())
        {
            objectInZone.Add(other.transform);
        }
        objectToSlowOnDash.Add(other.transform);

        if (player.GetComponent<Displacement>().IsDashing() && objectToSlowOnDash.Contains(other.transform))
        {
            other.GetComponent<Rigidbody2D>().velocity /= player.GetComponent<Displacement>().slowStrength;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            objectInZone.Remove(other.transform);
            if(other.transform == nearestElement?.transform)
            {
                ChangeNearestElementColor(false);
            }
        }
        if (player.GetComponent<Displacement>().IsDashing() && objectToSlowOnDash.Contains(other.transform))
        {
            other.GetComponent<Rigidbody2D>().velocity *= player.GetComponent<Displacement>().slowStrength;
        }
        objectToSlowOnDash.Remove(other.transform);

    }

    private void SortList()
    {
        objectInZone.Sort((t1, t2) => Vector3.Distance(t1.position, player.position).CompareTo(Vector3.Distance(t2.position, player.position)));
    }

    private Transform GetParentTransform(Transform t)
    {
        if (t.parent)
        {
            return GetParentTransform(transform.parent);
        }
        return null;
    }

    private Transform GetNearestElement()
    {
        if(objectInZone.Count > 0)
        {
            SortList();
            if(nearestElement == objectInZone[0])
            {
                nearestElement = objectInZone[0];
                ChangeNearestElementColor(true);
            }
            else
            {
                ChangeNearestElementColor(false);
                nearestElement = objectInZone[0];
            }
        }
        return null;
    }

    public void ChangeSpeedObjectInZone(bool isSlow)
    {
        foreach (var item in objectToSlowOnDash)
        {
            item.GetComponent<Rigidbody2D>().velocity *= isSlow ? 1/ player.GetComponent<Displacement>().slowStrength : player.GetComponent<Displacement>().slowStrength;
        }
    }

    private void ChangeNearestElementColor(bool isBlack)
    {
        if (nearestElement)
        {
            nearestElement?.GetComponent<MeshRenderer>().material.DOColor(isBlack ? new Color(0, 0, 0) : new Color(190, 27, 0) / 255 * 2, "Color_15CF1060", 1.0f);
            nearestElement?.GetComponent<MeshRenderer>().material.DOColor(isBlack ? new Color(0, 0, 0) : new Color(100, 13, 25) / 255 * 2, "Color_B93BCC95", 1.0f);
        }

    }

}
