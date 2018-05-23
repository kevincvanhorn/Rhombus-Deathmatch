using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShipBase : MonoBehaviour
{

    /* Serialized Attributes: */
    public Bullet bulletPrefab;
    public float moveSpeed = 10;

    /* Hidden Public Components: */
    [HideInInspector]
    public new Collider2D collider;
    [HideInInspector]
    public Bounds bounds;

    /* Protected Attributes: */
    protected new Rigidbody2D rigidbody;
    protected int numBulletsPerTurn = 1;      // The number of bullets allowed in an attack phase
    protected int numBulletsSpawned = 0;
    protected Bullet activeBullet = null;     // The current refereneced active bullet
    protected bool hasBulletSpawned = false;
    protected bool canBulletChangeDir = true; // Can the bullet change directions after being spawned?
    
    // Use this for initialization
    protected virtual void Start()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        bounds = collider.bounds;
    }

    /* Launches an object of type <Bullet> from the ship. */
    public virtual void LaunchBulletSingle(Vector2 swipeDirection, Bullet _bullet)
    {
        if (activeBullet == null)
        {
            activeBullet = (Bullet)Instantiate(_bullet, transform.position, transform.rotation);
            activeBullet.moveDirection = swipeDirection;

            canBulletChangeDir = true;
        }
    }
}
