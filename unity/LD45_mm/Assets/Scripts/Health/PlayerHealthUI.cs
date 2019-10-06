using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthUI : HealthUI
{
    public GameObject[] hearts;
    public static PlayerHealthUI instance;
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    public override void UpdateUI(HealthScript hs)
    {
        base.UpdateUI(hs);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < hs.CurrentHP);
        }
    }
    public override void InitUI(HealthScript hs)
    {
        base.InitUI(hs);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        Destroy(GetComponent<ContentSizeFitter>());
    }
}
