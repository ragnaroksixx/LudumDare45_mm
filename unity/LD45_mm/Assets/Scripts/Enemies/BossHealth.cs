using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    public override void Die()
    {
        this.transform.root.gameObject.GetComponent<PalletteController>().Death();
    }
    public override void TakeDamage(int damage)
    {
        if (isArmored && damage == 1)
            damage = 0;
        if( this.transform.root.gameObject.GetComponent<PalletteController>().TakeDamage() == false) return;
        base.TakeDamage(damage);
    }
}
