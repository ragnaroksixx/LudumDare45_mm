using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdFlier : MonoBehaviour
{
    public Transform start, end;
    public static BirdFlier instance;

    private void Awake()
    {
        instance = this;
    }

    public void Fly()
    {
        transform.position = start.position;

        transform.DOMove(end.position, 10);
    }
}
