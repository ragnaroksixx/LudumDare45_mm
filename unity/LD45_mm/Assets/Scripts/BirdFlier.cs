using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdFlier : MonoBehaviour
{
    public CanvasGroup canvas;
    public static BirdFlier instance;

    private void Awake()
    {
        instance = this;
    }

    public void Fly()
    {
        canvas.DOFade(1, 1).SetDelay(8);
    }
}
