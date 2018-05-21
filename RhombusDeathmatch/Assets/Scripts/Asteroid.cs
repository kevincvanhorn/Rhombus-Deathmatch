using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

    //private float slowTime = 0.5f;
    //private float slowAmount = 1.0f;
    private Vector2 impactVelocity = Vector2.zero; // The speed of the bullet on impact;
    
    private Renderer rend;
    private new Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        if(rigidbody.velocity.magnitude < 0.1 && rigidbody.velocity !=Vector2.zero)
        {
            rigidbody.velocity = Vector2.zero;
            GameManager.Instance.NextTurn(); // Post semaphore to transition;
        }
    }

    void OnHit()
    {
        rend.material.SetColor("_Color",new Color(255,255,255));
        rigidbody.AddForce(impactVelocity);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogError("ashfaklsjdsfk77");
        if(collision.gameObject.GetComponent<Bullet>() is Bullet)
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            impactVelocity = bullet.moveSpeed * bullet.moveDirection.normalized * 50; // 50 is the speed factor to boost the asteroid on impact.

            //impactVelocity.x = bullet.moveSpeed * bullet.moveDirection.normalized.x;
            //impactVelocity.y = bullet.moveSpeed *  bullet.moveDirection.normalized.y;

            bullet.moveSpeed = 0;
            OnHit();
            //StartCoroutine(SlowTime());
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
