using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerZone : MonoBehaviour
{

    public float distanceToCatch = 1.0f;


    private Transform player;
    private ObjectHandler playerObjectHandler;

    private List<Transform> objectInZone;
    private Transform nearestElement;

    private PlayerInput input;

    private void Awake()
    {
        input = GetComponentInParent<PlayerInput>();

        input.actions.Enable();

        input.currentActionMap["CaughtObject"].performed += context => OnCaughtObject(context);

    }



    // Start is called before the first frame update
    void Start()
    {
        objectInZone = new List<Transform>();

        player = GetComponentInParent<Displacement>().transform;
        playerObjectHandler = player.GetComponent<ObjectHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        GetNearestElement();

        if(nearestElement && Vector2.Distance(nearestElement.position, transform.position) <= distanceToCatch)
        {
            CaughtObject();
        }
    }

    private void OnCaughtObject(InputAction.CallbackContext obj)
    {
        CaughtObject();
    }

    public void CaughtObject()
    {
        if (!playerObjectHandler.GetObjectHandled())
        {
            ChangeNearestElementColor(false);
            playerObjectHandler.SetObjectHandled(nearestElement);
            nearestElement = null;
            objectInZone.RemoveAt(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && other.transform != playerObjectHandler.GetObjectHandled())
        {
            objectInZone.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            objectInZone.Remove(other.transform);
            if(other.transform == nearestElement?.transform)
            {
                ChangeNearestElementColor(false);
            }
        }
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

    private void ChangeNearestElementColor(bool isBlack)
    {
        nearestElement?.GetComponent<MeshRenderer>().material.DOColor(isBlack ? new Color(0, 0, 0) : new Color(190, 27, 0) / 255 * 2, "Color_15CF1060", 1.0f);
        nearestElement?.GetComponent<MeshRenderer>().material.DOColor(isBlack ? new Color(0, 0, 0) : new Color(100, 13, 25) / 255 * 2, "Color_B93BCC95", 1.0f);
    }

}
