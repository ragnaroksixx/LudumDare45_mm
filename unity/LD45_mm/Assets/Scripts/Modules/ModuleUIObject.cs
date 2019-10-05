using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ModuleUIObject : MonoBehaviour
{
    public Type moduleType;
    public Module module;
    public TMP_Text text;
    public Button addButton, removeButton;
    ModuleSystem moduleSystem;
    public Outline outline;
    Color outlineColor;
    Tweener shakeTween;
    public void Init(Type t, Module m, ModuleSystem ms)
    {
        outlineColor = outline.effectColor;
        moduleType = t;
        module = m;
        moduleSystem = ms;
        text.text = m.Name;

        addButton.onClick.AddListener(OnAdd);
        removeButton.onClick.AddListener(OnRemove);

        ModuleUIMenu.instance.AllUIelements.Add(t, this);
    }
    public void AllowEdit(bool val)
    {
        if (val)
        {
            if (moduleSystem.HasModule(moduleType))
            {
                ShowRemove(true);
                ShowAdd(false);
            }
            else
            {
                ShowRemove(false);
                ShowAdd(true);
            }
        }
        else
        {
            ShowAdd(false);
            ShowRemove(false);
        }
    }
    public void OnAdd()
    {
        if (!moduleSystem.CanAddComponent(moduleType))
        {
            CantAddEffect();
            return;
        }

        moduleSystem.ActivateModule(moduleType);
        transform.SetParent(moduleSystem.attachedList, false);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        LayoutRebuilder.ForceRebuildLayoutImmediate(moduleSystem.attachedList.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(moduleSystem.unattachedList.transform as RectTransform);

        ShowAdd(false);
        ShowRemove(true);
    }

    public void OnRemove()
    {
        moduleSystem.DeactivateModule(moduleType);
        transform.SetParent(moduleSystem.unattachedList, false);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        LayoutRebuilder.ForceRebuildLayoutImmediate(moduleSystem.attachedList.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(moduleSystem.unattachedList.transform as RectTransform);

        ShowAdd(true);
        ShowRemove(false);
    }

    public void ShowAdd(bool show)
    {
        addButton.gameObject.SetActive(show);
    }

    public void ShowRemove(bool show)
    {
        removeButton.gameObject.SetActive(show);
    }

    public void CantAddEffect()
    {
        if (shakeTween != null && !shakeTween.IsComplete()) return;
        outline.effectColor = Color.red;
        shakeTween = outline.transform.DOShakePosition(.5f, 3).OnComplete(() => { outline.effectColor = outlineColor; }).SetAutoKill(false);
        ModuleUIMenu.instance.HighlightCount(Color.red, .5f);
    }
}
