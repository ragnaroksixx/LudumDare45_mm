using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    public CanvasGroup g;
    protected virtual void Awake()
    {
        g = GetComponent<CanvasGroup>();
        if (g == null)
            g = gameObject.AddComponent<CanvasGroup>();
    }
    public virtual void UpdateUI(HealthScript hs)
    {

    }
    public virtual void InitUI(HealthScript hs)
    {
        UpdateUI(hs);
    }

    public void Show()
    {
        g.alpha = 1;
    }

    public void Hide()
    {
        g.alpha = 0;
    }
}
