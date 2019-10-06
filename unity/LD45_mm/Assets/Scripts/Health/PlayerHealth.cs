using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerHealth : HealthScript
{
    public CanvasGroup gameOverUI;
    public bool invincible = false;
    public static PlayerHealth instance;
    private void Awake()
    {
        instance = this;
    }
    public override void Die()
    {
        base.Die();
        Time.timeScale = 0.01f;
        VisionModule.DisableSight();
        gameOverUI.DOFade(1, 0.01f);
    }

    public override void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if (ModuleSystem.instance.currentModules.Count == 0) return;
        TakeDameUI();
        if (invincible) return;
        StartCoroutine(Inviniciblity());
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

            mpu.DestroyIn(30);
        }

        if (ModuleSystem.instance.currentModules.Count == 0)
            Die();
    }

    IEnumerator Inviniciblity()
    {
        invincible = true;
        yield return new WaitForSeconds(1);
        invincible = false;
    }
}
