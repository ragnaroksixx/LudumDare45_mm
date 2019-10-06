using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ModuleAquiredUI : MonoBehaviour
{
    public Transform offScreenPos, onScreenPos;
    public TMP_Text text,hint;
    public static ModuleAquiredUI instance;

    Sequence s;
    private void Awake()
    {
        instance = this;
        Hide(0);
    }
    public Tween Show(float duration)
    {
        return transform.DOLocalMove(onScreenPos.localPosition, duration);
    }

    public Tween Hide(float duration)
    {
        return transform.DOLocalMove(offScreenPos.localPosition, duration);
    }
    public void Aquire(Module m)
    {
        if (SavedData.HasCollectedModule(m.GetType())) return;

        string t = "Aquired: " + m.Name;
        text.text = t;
        hint.text = m.GetTip();

        if (s != null && !s.IsComplete()) s.Kill();

        s = DOTween.Sequence();
        Hide(0);
        s.Append(Show(.5f));
        s.Append(Hide(1).SetDelay(3f));
    }
}
