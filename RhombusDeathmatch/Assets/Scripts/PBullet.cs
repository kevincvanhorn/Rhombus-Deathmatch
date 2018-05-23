using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBullet : Bullet {

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GravityField"))
        {
            moveSpeed *= 1.8f;

        }
        else if (collision.gameObject.GetComponent<Asteroid>() is Asteroid)
        {
            OnHitAsteroid();
            Asteroid asteroidHit = collision.gameObject.GetComponent<Asteroid>();

            asteroidHit.impactVelocity = moveSpeed * moveDirection.normalized * 50;
            moveSpeed = 0;
            //asteroidHit.state = AsteroidState
            asteroidHit.HitByBullet();
        }
        else if (collision.CompareTag("Boundary"))
        {
            rend.enabled = false;
            moveSpeed = 0;
        }
    }
}
