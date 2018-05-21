﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float moveSpeed = 10;
    public float moveDist = 5;
    public Vector3 moveDirection;
    public new Rigidbody2D rigidbody;

    Coroutine thisCoroutine = null;
    
    private float lifeTime;
    private Renderer rend;

	// Use this for initialization
	private void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        lifeTime = moveDist / moveSpeed; // t = d/r

        thisCoroutine = StartCoroutine(DestroyViaLifeTime());
	}

    private void Update()
    {
        //transform.Translate(transform.position *speed);
        Vector3 pos = rigidbody.position;
        float deltaX = moveSpeed * Time.deltaTime * moveDirection.normalized.x;
        float deltaY = moveSpeed * Time.deltaTime * moveDirection.normalized.y;
        rigidbody.MovePosition(new Vector3(pos.x + deltaX, pos.y + deltaY, pos.z));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<AGravityField>() is AGravityField)
        {
            moveSpeed *= 2;
        }
        else if (collision.gameObject.GetComponent<Asteroid>() is Asteroid)
        {
            OnHitAsteroid();
        }
        
    }

    private void OnHitAsteroid()
    {
        /* Stop Asteroid in place & let trail catch up. */
        StopCoroutine(thisCoroutine);
        rend.enabled = false; 
        //moveSpeed = 0;

        GameManager.Instance.DelayAttackTransition();
        StartCoroutine(DestroyOnDelay(1f)); // delay for trail to execute.
    }

    private IEnumerator DestroyOnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private IEnumerator DestroyViaLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        GameManager.Instance.NextTurn(); // Attack Missed all Asteroids: Should transition.
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();   
    }
}
