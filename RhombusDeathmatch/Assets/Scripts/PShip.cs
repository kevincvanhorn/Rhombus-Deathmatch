using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PShip : ShipBase {

    private bool canPlayerMove = false;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnSimpleSwipe(Vector2 swipeDirection, Vector2 endpoint)
    {
        if(GameManager.State == GameState.BulletTurn)
        {
            if (numBulletsSpawned == 1 && canBulletChangeDir)
            {
                activeBullet.moveDirection = swipeDirection;
                canBulletChangeDir = false;
            }
            else if(numBulletsSpawned < numBulletsPerTurn)
            {
                numBulletsSpawned++;
                LaunchBulletSingle(swipeDirection);
            }
                
        }
        else if(GameManager.State == GameState.MoveTurn)
        {
            /* Start Movement & Freeze input. */
            GameManager.Instance.RequestRestrictInput();
            //rigidbody.AddForce(moveSpeed*swipeDirection); // Impulse
            StartCoroutine(MoveTo(swipeDirection, endpoint));

            /* Reset Attack vars. */
            numBulletsSpawned = 0;
        }
    }

    public void LaunchBulletSingle(Vector2 swipeDirection)
    {
        base.LaunchBulletSingle(swipeDirection, bulletPrefab);
    }

    private IEnumerator MoveTo(Vector2 moveDirection, Vector2 target)
    {
        target = Camera.main.ScreenToWorldPoint(target); // For using mouse.
        Vector3 startPos = rigidbody.position;
        float changeRate = moveSpeed / Vector2.Distance(startPos, target);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime*changeRate;
            //transform.position = Vector3.Lerp(startPos, target, Mathf.SmoothStep(0f, 1f, t));
            rigidbody.MovePosition(Vector3.Lerp(startPos, target, Mathf.SmoothStep(0f,1f,t)));
            yield return null;
        }
        GameManager.Instance.NextTurn(); // At end of movement
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            GameManager.Instance.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    /*IEnumerator MoveTo(Vector2 moveDirection, Vector2 target)
    {
        target = Camera.main.ScreenToWorldPoint(target); // For using mouse.
        Vector3 pos = rigidbody.position;
        float moveDist = (target - (Vector2)pos).magnitude;
        float distTraveled = 0f;

        while (distTraveled < moveDist)
        {
            pos = rigidbody.position;
            float deltaX = moveSpeed * Time.deltaTime * moveDirection.normalized.x;
            float deltaY = moveSpeed * Time.deltaTime * moveDirection.normalized.y;
            Vector3 moveVector = new Vector3(pos.x + deltaX, pos.y + deltaY, pos.z);
            rigidbody.MovePosition(moveVector);
            distTraveled += (new Vector2(deltaX, deltaY)).magnitude;
            yield return new WaitForEndOfFrame();
        }
        GameManager.Instance.NextTurn(); // At end of movement
        //GameManager.Instance.RequestAllowInput();

    }*/
}
