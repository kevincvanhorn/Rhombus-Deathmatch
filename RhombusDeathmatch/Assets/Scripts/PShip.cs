using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PShip : MonoBehaviour {

    public Bullet bulletPrefab;
    //public float moveForce = 50;
    public float moveSpeed = 10;

    [HideInInspector]
    public new Collider2D collider;
    [HideInInspector]
    public Bounds bounds;
    private int numBulletsPerTurn = 1; // The number of bullets allowed in an attack phase
    private int numBulletsSpawned = 0;


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
            //rigidbody.AddForce(moveForce*swipeDirection); // Impulse
            StartCoroutine(MoveTo(swipeDirection, endpoint));

            /* Reset Attack vars. */
            numBulletsSpawned = 0;
        }
    }

    public void LaunchBulletSingle(Vector2 swipeDirection)
    {
        if (activeBullet == null)
        {
            activeBullet = (Bullet)Instantiate(bulletPrefab, transform.position, transform.rotation);
            activeBullet.moveDirection = swipeDirection;

            canBulletChangeDir = true;
        }
    }

    IEnumerator MoveTo(Vector2 moveDirection, Vector2 target)
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
        GameManager.Instance.RequestAllowInput();

    }

}
