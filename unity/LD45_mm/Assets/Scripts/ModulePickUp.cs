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

    public SpriteRenderer glow;
    Sequence gSeqScale, gSeqOpac;

    private void Update()
    {
        canGrabTime -= Time.deltaTime;
    }
    private void Start()
    {
        gSeqScale = DOTween.Sequence();
        glow.transform.localScale = Vector3.one * 2;
        float time = 0.9f;
        float delay = 0.1f;
        gSeqScale.Append(glow.transform.DOScale(0, time).SetDelay(delay * 2));
        gSeqScale.Append(glow.transform.DOScale(2f, time).SetDelay(delay));
        gSeqScale.SetLoops(-1);

        gSeqOpac = DOTween.Sequence();
        Color c = glow.color;
        gSeqOpac.Append(glow.DOColor(new Color(), time).SetDelay(delay * 2));
        gSeqOpac.Append(glow.DOColor(c, time).SetDelay(delay));
        gSeqOpac.SetLoops(-1);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canGrabTime <= 0 && collision.gameObject.tag == "Player")
        {
            if (ModuleSystem.instance.HasModule<CoreModule>() || moduleType == typeof(CoreModule))
            {
                if (ModuleSystem.instance.HasModule<WalkModule>() || moduleType == typeof(WalkModule) || module is FullSightModule || module is MonochromeModule)
                {
                    ModuleAquiredUI.instance.Aquire(module);
                    ModuleSystem.instance.AddCollectedModule(moduleType);
                    Destroy(this.gameObject);
                }
            }
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

