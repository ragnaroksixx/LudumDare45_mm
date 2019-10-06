using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using TMPro;

public class ModulePickUp : MonoBehaviour
{
    public Type moduleType;
    public Module module;
    public float canGrabTime = 1;
    public TMP_Text text;
    bool destroy;
    Sequence s;
    public SpriteRenderer image_1, image_2;

    private void Update()
    {
        canGrabTime -= Time.deltaTime;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canGrabTime <= 0 && collision.gameObject.tag == "Player")
        {
            ModuleSystem.instance.AddCollectedModule(moduleType);
            Destroy(this.gameObject);
        }
    }


    public static ModulePickUp Spawn(Type t, Module m, Pose p)
    {
        ModulePickUp mp = GameObject.Instantiate(ModuleSystem.instance.moduleObjectPrefab, p.position, p.rotation);
        mp.moduleType = t;
        mp.module = m;
        mp.text.text = m.Name[0].ToString().ToUpper();
        return mp;
    }

    public void DestroyIn(float x)
    {
        if (destroy) return;
        destroy = true;
        Destroy(this.gameObject, x);

        Color c1 = image_1.color;
        Color c2 = image_2.color;
        Color trans = new Color();
        s = DOTween.Sequence();
        s.Append(image_1.DOColor(c1, 0f));
        s.Append(image_2.DOColor(c2, 0f).SetDelay(0.1f));
        s.Append(image_1.DOColor(trans, 0.1f));
        s.Append(image_2.DOColor(trans, 0.1f).SetDelay(0.1f));
        s.SetLoops(-1);
        s.SetAutoKill(false);
    }
}

