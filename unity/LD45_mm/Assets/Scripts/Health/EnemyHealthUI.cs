using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthUI : HealthUI
{
    public Image barImage;

    public override void UpdateUI(HealthScript hs)
    {
        base.UpdateUI(hs);
        barImage.fillAmount = (float)hs.CurrentHP / (float)hs.maxHP
 ;
    }
}
