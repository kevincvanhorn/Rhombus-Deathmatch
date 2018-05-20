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
        }
    }
}
