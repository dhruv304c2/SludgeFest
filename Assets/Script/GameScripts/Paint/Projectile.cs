using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer ProjectileSprite;
    [SerializeField] ParticleSystem particles;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Walls"))
        {
            Destroy(ProjectileSprite);
            particles.Stop();
            Destroy(gameObject.GetComponent<Collider2D>());
            Destroy(gameObject, 2f);
        }
    }
    
}
