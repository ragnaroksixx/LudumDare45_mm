using UnityEngine;
using System.Collections;

public class EnemyHealth : HealthScript
{
    public bool isArmored = false;

    public override void Die()
    {
        base.Die();
        Destroy(this.transform.root.gameObject);
    }
    public override void TakeDamage(int damage)
    {
        if (isArmored && damage == 1)
            damage = 0;
        base.TakeDamage(damage);
    }
}
