using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerBullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    SpriteRenderer sr;
    int damage = 1;

    public int Damage { get => damage; set => damage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
        if (collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.GetComponentInParent<EnemyHealth>())
            collision.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(damage);
        } 
    }

    private void OnDestroy()
    {
        ParticleSystem p = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        MainModule mm = p.main;
        if (sr)
            mm.startColor = sr.color;
    }

    public void SetColor(Color c)
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = c;

    }
}
