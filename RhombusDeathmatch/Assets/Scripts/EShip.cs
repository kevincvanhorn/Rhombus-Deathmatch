using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EShip : ShipBase {

    private PShip player;
    private Vector2[] moveLocations;
    private int curLocation = 0;
    private Asteroid[] asteroids;
    private System.Random random;

    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<PShip>();
        moveSpeed = player.moveSpeed;
        moveLocations = new Vector2[] {new Vector2(0,-3), new Vector2(0, 3.1f), new Vector2(-6, 2.2f), new Vector2(-4.5f, -4), new Vector2(-1.8f, 0), new Vector2(1, -4) };
        asteroids = GameObject.FindObjectsOfType<Asteroid>();

        random = new System.Random();
    }

    public void DoEnemyAttack()
    {
        //LaunchBulletSingle(Vector2.up);
        int randInt = random.Next(0, asteroids.Length - 1);
        LaunchBulletSingle(asteroids[randInt].transform.position);
    }

    public void DoEnemyMove()
    {
        StartCoroutine(MoveTo(moveLocations[curLocation]));
        curLocation++;
        if (curLocation >= moveLocations.Length) curLocation = 0;
    }

    private IEnumerator MoveTo(Vector2 target)
    {
        Vector3 startPos = rigidbody.position;
        float changeRate = moveSpeed / Vector2.Distance(startPos, target);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * changeRate;
            //transform.position = Vector3.Lerp(startPos, target, Mathf.SmoothStep(0f, 1f, t));
            rigidbody.MovePosition(Vector3.Lerp(startPos, target, Mathf.SmoothStep(0f, 1f, t)));
            yield return null;
        }
        GameManager.Instance.NextTurn(); // At end of movement
    }

    /* Launch a single enemy bullet specified by the bulletPrefab object. */
    private void LaunchBulletSingle(Vector2 target)
    {
        Vector2 swipeDirection = target - (Vector2)rigidbody.transform.position;
        base.LaunchBulletSingle(swipeDirection, bulletPrefab);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>() != null)
        {
            GameManager.Instance.OnEnemyDeath();
            Destroy(gameObject);
        }
    }

}
