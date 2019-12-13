using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackHole : MonoBehaviour
{

    public float timeToExplode;
    public float timeToScaleUp;

    public float maxScaleSize;
    public float maxBloomIntensity;

    public float delay;

    public AnimationCurve blackHoleEaseColor;
    public AnimationCurve blackHoleEaseScale;

    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;

        StartCoroutine(StartExplosion());
       
    }
    
     private IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(delay);
        transform.DOScale(maxScaleSize, timeToScaleUp).SetEase(blackHoleEaseScale);

        mat.DOFloat(maxBloomIntensity, "_PowerColor", timeToExplode).SetEase(blackHoleEaseColor);
    }
}
