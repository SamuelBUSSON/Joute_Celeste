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
        
        AkSoundEngine.PostEvent("Play_Trou_Noir", gameObject);

        mat.DOFloat(maxBloomIntensity, "_PowerColor", timeToExplode).SetEase(blackHoleEaseColor).OnComplete(() => EndRound());
    }

    private void EndRound()
    {
        Debug.Log("EndRound");
        AkSoundEngine.SetSwitch("Hit_or_Lancer_or_Mort", "Mort", gameObject);
        AkSoundEngine.PostEvent("Play_Palier_Voix", gameObject);

        if ( GameManager.Instance.player1.GetComponent<PlayerHealth>().Health > GameManager.Instance.player2.GetComponent<PlayerHealth>().Health)
        {
            GameManager.Instance.player2.GetComponent<PlayerHealth>().isDead = true;
            GameManager.Instance.WinLoose(GameManager.Instance.player2.playerIndex);
        }
        else
        {
            if (GameManager.Instance.player1.GetComponent<PlayerHealth>().Health == GameManager.Instance.player2.GetComponent<PlayerHealth>().Health)
            {
                GameManager.Instance.player1.GetComponent<PlayerHealth>().isDead = true;
                GameManager.Instance.player2.GetComponent<PlayerHealth>().isDead = true;
                GameManager.Instance.Draw();
            }
            else
            {
                GameManager.Instance.player1.GetComponent<PlayerHealth>().isDead = true;
                GameManager.Instance.WinLoose(GameManager.Instance.player1.playerIndex);
            }
        }
        Destroy(gameObject);
    }
}
