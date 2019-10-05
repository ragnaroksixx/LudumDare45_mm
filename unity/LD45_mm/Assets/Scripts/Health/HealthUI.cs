using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    public virtual void UpdateUI(HealthScript hs)
    {

    }
    public virtual void InitUI(HealthScript hs)
    {
        UpdateUI(hs);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
