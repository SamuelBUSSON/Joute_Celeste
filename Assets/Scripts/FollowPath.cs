using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowPath : MonoBehaviour
{

    public Transform[] paths;

    private int currentPos = 0;
    private bool moveFinish = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveFinish)
        {
            moveFinish = true;
            transform.DOMove(paths[currentPos].position, 1.0f).OnComplete(() => CheckIndex());
        }

    }

    private void CheckIndex()
    {
        currentPos = currentPos == paths.Length - 1 ? 0 : currentPos + 1;
        moveFinish = false;

    }
}
