using UnityEngine;
using System.Collections;

public class PlayerHealth : HealthScript
{

    public override void Die()
    {
        base.Die();
        Debug.LogWarning("ded");
    }
}
