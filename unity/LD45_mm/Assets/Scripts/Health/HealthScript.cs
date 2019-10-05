using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HealthScript : MonoBehaviour
{
    public int maxHP;
    private int currentHP;
    public HealthUI ui;

    public int CurrentHP { get => currentHP; }

    public SpriteRenderer image;
    Color normal;
    Sequence s;
    public virtual void Start()
    {
        normal = image.color;
        currentHP = maxHP;
        if (ui)
            ui.InitUI(this);

    }
    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (ui)
            ui.UpdateUI(this);

        if (CurrentHP <= 0)
            Die();
        else
            TakeDameUI();
    }

    public virtual void Die()
    {

    }

    public void TakeDameUI()
    {
        Color c = Color.red;

        if (s != null && !s.IsComplete()) s.Kill(false);

        s = DOTween.Sequence();
        s.Append(image.DOColor(c, 0.1f));
        s.Append(image.DOColor(Color.white, 0.1f));
        s.SetLoops(5);
        s.SetAutoKill(false);
        s.OnComplete(() => { image.color = normal; });
    }
}
