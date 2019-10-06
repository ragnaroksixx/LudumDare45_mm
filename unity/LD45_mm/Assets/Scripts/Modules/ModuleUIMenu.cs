using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

public class ModuleUIMenu : MonoBehaviour
{
    public CanvasGroup attachedList, unattachedList;

    public Transform unattachedOnLocation, unattachedOffLocation;
    bool open;
    private Dictionary<Type, ModuleUIObject> allUIelements = new Dictionary<Type, ModuleUIObject>();
    public static ModuleUIMenu instance;

    public Dictionary<Type, ModuleUIObject> AllUIelements { get => allUIelements; set => allUIelements = value; }
    public bool IsOpen { get => open; set => open = value; }
    public CanvasGroup[] blackoutImages;
    public TMP_Text countText;
    Sequence s;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Close();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleMenu();
        }
    }
    public void ToggleMenu()
    {
        if (open)
            Close();
        else
            Open();
    }
    public void Open()
    {
        attachedList.interactable = true;
        unattachedList.interactable = true;

        unattachedList.transform.DOLocalMove(unattachedOnLocation.localPosition, 1);
        open = true;

        foreach (ModuleUIObject item in AllUIelements.Values)
        {
            item.AllowEdit(true);
        }
    }

    public void Close()
    {
        attachedList.interactable = false;
        unattachedList.interactable = false;

        unattachedList.transform.DOLocalMove(unattachedOffLocation.localPosition, 1);
        open = false;
        foreach (ModuleUIObject item in AllUIelements.Values)
        {
            item.AllowEdit(false);
        }
    }

    public ModuleUIObject FindUI(Type t)
    {
        return allUIelements[t];
    }

    public void UpdateCount(int value)
    {
        countText.text = "MODs " + value.ToString() + "/" + ModuleSystem.instance.maxComponents;
    }

    public void HighlightCount(Color c, float duration)
    {
        if (s != null && !s.IsComplete()) s.Kill(false);

        s = DOTween.Sequence();
        s.Append(countText.DOColor(c, 0f));
        s.Append(countText.DOColor(Color.white, 0.1f).SetDelay(duration));
        s.SetAutoKill(false);
    }
}
