using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    SpriteRenderer sr;
    int damage = 1;

    public int Damage { get => damage; set => damage = value; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 31 || collision.gameObject.layer == 28 || collision.gameObject.layer == 30)
        {
            Destroy(this.gameObject);
        }else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<PlayerMovement>().HitPlayer(transform);
            collision.gameObject.GetComponentInParent<HealthScript>().TakeDamage(this.Damage);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        ParticleSystem p = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    }

    public void SetColor(Color c)
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = c;

    }
}
