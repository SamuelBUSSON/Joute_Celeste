using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class SlowMo : MonoBehaviour
{

    private float frequency = 0.25f;
    private float destoryTime = 0.5f;
    

    public Material phantomMat;

    public PostProcessVolume postVolume;

    private Vignette vignette;


    //private float fov = 0.0f;
    private float intensity = 0.0f;
    private float timeScale = 1.0f;

    private float time;

    private bool activate = false;

    private void Start()
    {
        frequency = SlowmoManager.instance.frequency;
        destoryTime = SlowmoManager.instance.destoryTime;

        vignette = postVolume.profile.GetSetting<Vignette>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activate = !activate;
            SlowmoManager.instance.SetSlowMo(activate);
        }



        //cameraFreeLook.m_Lens.FieldOfView = fov;
        vignette.intensity.value = intensity;        

        if (SlowmoManager.instance.GetSlowMo())
        {            
            showGhost();
            //DOTween.To(() => fov, x => fov = x, 25f, 1);
            DOTween.To(() => intensity, x => intensity = x, 0.5f, 0.3f);
            Time.timeScale = 0.25f;
        }
        else
        {
            //DOTween.To(() => fov, x => fov = x, 30, 1);
            DOTween.To(() => intensity, x => intensity = x, 0f, 1);
            Time.timeScale = 1f;
        }

    }

    private void showGhost()
    {
        time += Time.deltaTime;
        if (time >= frequency)
        {
            GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
            Destroy(clone.GetComponent<Animator>());
            Destroy(clone.GetComponent<SlowMo>());
            Destroy(clone.GetComponent<CharacterController>());


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
