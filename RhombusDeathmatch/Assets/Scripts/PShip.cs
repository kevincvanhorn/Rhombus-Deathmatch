using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PShip : MonoBehaviour {

    public Bullet bulletPrefab;

    [HideInInspector]
    public new Collider2D collider;
    public Bounds bounds;

    private Bullet activeBullet = null;
    private bool hasBulletSpawned = false;
    private bool canBulletChangeDir = true;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider2D>();
        bounds = collider.bounds;

    }

    // Update is called once per frame
    void Update () {
	}

    public void OnSimpleSwipe(Vector2 swipeDirection)
    {
        if(GameManager.state == GameState.BulletTurn)
        {
            if (hasBulletSpawned && canBulletChangeDir)
            {
                activeBullet.moveDirection = swipeDirection;
                canBulletChangeDir = false;
            }
            else
                LaunchBulletSingle(swipeDirection);
        }
    }

    public void LaunchBulletSingle(Vector2 swipeDirection)
    {
        Debug.Log(activeBullet);
        if (activeBullet == null)
        {
            activeBullet = (Bullet)Instantiate(bulletPrefab, transform.position, transform.rotation);
            activeBullet.moveDirection = swipeDirection;

            hasBulletSpawned = true;
            canBulletChangeDir = true;
        }
    }
}
