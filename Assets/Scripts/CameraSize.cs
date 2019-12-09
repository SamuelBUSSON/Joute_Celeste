using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSize : MonoBehaviour
{

    private CinemachineTargetGroup targetGroup;

    public float minLength = 3.0f;
    public float maxLength = 25.0f;

    public float minSize = 6.5f;
    public float maxSize = 10.5f;

    public CinemachineVirtualCamera cinemachineVirtualCamera;


    // Start is called before the first frame update
    void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetGroup.IsEmpty)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(minSize, maxSize, Mathf.InverseLerp(minLength, maxLength, Vector3.Distance(targetGroup.BoundingBox.max, targetGroup.BoundingBox.min)));
        }


    }
}
