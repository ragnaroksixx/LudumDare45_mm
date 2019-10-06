using UnityEngine;
using System.Collections;

public class PlayerHealth : HealthScript
{

    public override void Die()
    {
        base.Die();
        Debug.LogWarning("ded");
    }
    public override void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        TakeDameUI();
        for (int i = 0; i < damage; i++)
        {
            Module m = ModuleSystem.instance.Pop();
            if (m == null) continue;
            ModuleUIMenu.instance.AllUIelements[m.GetType()].Uninit();
            m.DeactivateModule();
            Pose p = new Pose(transform.position, Quaternion.identity);
            ModulePickUp mpu = ModulePickUp.Spawn(m.GetType(), m, p);
            Vector2 dir = Random.insideUnitCircle * 20;
            dir.y = Mathf.Clamp(dir.y, 7, 12);
            dir.x = Mathf.Clamp(dir.x, -12, 12);
            mpu.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);

            mpu.DestroyIn(5);
        }
    }
}
