using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour {
    public Image image;
    public float duration = 0.1f;

    public void FadeIn(){
        image.DOFillAmount(1f, duration);
    }

    public void FadeOut(){

        image.DOKill();

        image.DOFillAmount(0f, duration);
    }
}