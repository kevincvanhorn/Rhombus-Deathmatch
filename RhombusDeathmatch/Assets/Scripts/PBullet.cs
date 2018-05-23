using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBullet : Bullet {

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.GetComponent<Asteroid>() is Asteroid)
        {
            OnHitAsteroid();
            Asteroid asteroidHit = collision.gameObject.GetComponent<Asteroid>();

            asteroidHit.impactVelocity = moveSpeed * moveDirection.normalized * 50;
            moveSpeed = 0;
            asteroidHit.state = AsteroidState.Player;
            asteroidHit.HitByBullet(this);
        }
    }
}
