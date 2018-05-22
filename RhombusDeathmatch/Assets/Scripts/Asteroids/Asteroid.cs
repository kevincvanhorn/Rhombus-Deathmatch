using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

    //private float slowTime = 0.5f;
    //private float slowAmount = 1.0f;
    public Vector2 impactVelocity = Vector2.zero; // The speed of the bullet on impact;
    
    private Renderer rend;
    public new Rigidbody2D rigidbody;
    private bool hasCollidedWithObject = false;

    // Use this for initialization
    void Awake () {
        rend = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        if(rigidbody.velocity.magnitude < 0.1 && rigidbody.velocity !=Vector2.zero)
        {
            rigidbody.velocity = Vector2.zero;
            //OnTurnReset();
            GameManager.Instance.NextTurn(); // Semaphore wait() for transition;
        }
    }

    void OnHit()
    {
        if (!hasCollidedWithObject)
        {
            Debug.LogError("HIT ASTEROID ");

            rend.material.SetColor("_Color", new Color32(255, 255, 255,255));
            rigidbody.AddForce(impactVelocity);
        }
    }


    public void OnTurnReset()
    {
        Debug.Log("Should be dull");
        rend.material.SetColor("_Color", new Color32(124, 124, 124,255));
        hasCollidedWithObject = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if(collision.gameObject.GetComponent<Bullet>() is Bullet)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            impactVelocity = bullet.moveSpeed * bullet.moveDirection.normalized * 50; // 50 is the speed factor to boost the asteroid on impact.
            bullet.moveSpeed = 0;
            OnHit();
            //StartCoroutine(SlowTime());
            hasCollidedWithObject = true;
        }*/
        
    }

    public void HitByBullet()
    {
        OnHit();
        //StartCoroutine(SlowTime());
        hasCollidedWithObject = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>() is Asteroid)
        {
            Asteroid asteroidHit = collision.gameObject.GetComponent<Asteroid>();
            asteroidHit.impactVelocity = impactVelocity;
            asteroidHit.OnHit();
        }
    }

    /*IEnumerator SlowTime()
    {
        Debug.LogError("Before: " + Time.timeScale);

        Time.timeScale = slowAmount;
        //Time.fixedDeltaTime = 0.2f * Time.timeScale; // Recommended to also lower Time.fixedDeltaTime by the same amount.

        Debug.LogError("During: " + Time.timeScale);
        yield return new WaitForSeconds(slowTime);
        Time.timeScale = 1.0f;
        //Time.fixedDeltaTime = 0.2f * Time.timeScale;
        Debug.LogError("After: " + Time.timeScale);

    }*/

}
