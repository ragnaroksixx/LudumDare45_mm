using UnityEngine;
using System.Collections;

public class EnemyHitbox : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<PlayerMovement>().HitPlayer(transform);
            collision.gameObject.GetComponentInParent<HealthScript>().TakeDamage(1);
        }
    }
}
