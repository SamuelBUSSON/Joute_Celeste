using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlowMoMob : MonoBehaviour
{

    private float frequency = 0.25f;
    private float destoryTime = 0.5f;
    public Material phantomMat;

    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        frequency = SlowmoManager.instance.frequency;
        destoryTime = SlowmoManager.instance.destoryTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (SlowmoManager.instance.GetSlowMo())
        {
            showGhost();
        }
    }


    private void showGhost()
    {
        time += Time.deltaTime;
        if (time >= frequency)
        {
            GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
            Destroy(clone.GetComponent<Animator>());
            Destroy(clone.GetComponent<SlowMoMob>());

            Collider[] colliders = clone.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }


            SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList)
            {
                smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                smr.material = phantomMat;
                smr.material.DOFloat(0, "Vector1_E932A045", destoryTime).OnComplete(() => Destroy(clone));
            }

            MeshRenderer[] skinMeshListChild = clone.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in skinMeshListChild)
            {
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                mr.material = phantomMat;
                mr.material.DOFloat(0, "Vector1_E932A045", destoryTime).OnComplete(() => Destroy(clone));
            }


            time = 0.0f;
        }
    }
}
