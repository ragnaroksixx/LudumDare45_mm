using UnityEngine;
using System.Collections;

public class playerAnimationController : MonoBehaviour
{
    public SpriteRenderer playerNoGun, playerGun,
        hat, rocket, gun, bigGun, sound;

    public static playerAnimationController instance;
    private void Awake()
    {
        instance = this;
    }
    public void SetVisibility(bool show, SpriteRenderer sr)
    {
        sr.gameObject.SetActive(show);
    }
}
