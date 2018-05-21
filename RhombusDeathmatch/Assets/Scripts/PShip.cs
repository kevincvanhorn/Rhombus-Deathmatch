using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PShip : MonoBehaviour {

    public Bullet bulletPrefab;
    public float moveForce = 50;

    [HideInInspector]
    public new Collider2D collider;
    [HideInInspector]
    public Bounds bounds;


    private Rigidbody2D rigidbody;
    private Bullet activeBullet = null;
    private bool hasBulletSpawned = false;
    private bool canBulletChangeDir = true;
    private bool canPlayerMove = false;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        bounds = collider.bounds;
    }

    // Update is called once per frame
    void Update () {
       
        
    }

    public void OnSimpleSwipe(Vector2 swipeDirection)
    {
        if(GameManager.State == GameState.BulletTurn)
        {
            if (hasBulletSpawned && canBulletChangeDir)
            {
                activeBullet.moveDirection = swipeDirection;
                canBulletChangeDir = false;
            }
            else
                LaunchBulletSingle(swipeDirection);
        }
        else if(GameManager.State == GameState.MoveTurn)
        {
            rigidbody.AddForce(moveForce*swipeDirection);
        }
    }

    public void LaunchBulletSingle(Vector2 swipeDirection)
    {
            if (activeBullet == null)
        {
            activeBullet = (Bullet)Instantiate(bulletPrefab, transform.position, transform.rotation);
            activeBullet.moveDirection = swipeDirection;

            hasBulletSpawned = true;
            canBulletChangeDir = true;
        }
    }


}
