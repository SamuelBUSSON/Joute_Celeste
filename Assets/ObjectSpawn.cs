using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject starPrefab;

    public float distance = 1.0f;

    private Transform currentChild;
    private float angle = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount != 1)
        {
            currentChild = Instantiate(starPrefab, transform.position, Quaternion.identity).transform;

            currentChild.SetParent(transform);

            currentChild.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        }
        else
        {
            float x = Input.GetAxis("ControllerVertical");
            float y = Input.GetAxis("ControllerHorizontal");
            if (x != 0.0f || y != 0.0f)
            {
                angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                Debug.Log(angle);
            }
        }
        
    }
}
