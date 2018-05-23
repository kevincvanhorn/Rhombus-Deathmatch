using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidState{
    Neutral, 
    Enemy,
    Player
}

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

    //private float slowTime = 0.5f;
    //private float slowAmount = 1.0f;
    public Vector2 impactVelocity = Vector2.zero; // The speed of the bullet on impact;

    [HideInInspector]
    public AsteroidState state = AsteroidState.Neutral;

    private Renderer rend;
    public new Rigidbody2D rigidbody;
    private bool hasCollidedWithObject = false;
    public Color color;

    // Use this for initialization
    void Awake () {
        rend = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        color = new Color32(124, 124, 124, 255);
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
            //rend.material.SetColor("_Color", new Color32(255, 255, 255,255));
            rend.material.SetColor("_Color",color);
            rigidbody.AddForce(impactVelocity);
        }
    }


    public void OnTurnReset()
    {
        rend.material.SetColor("_Color", new Color32(124, 124, 124,255));
        hasCollidedWithObject = false;
    }

    public void HitByBullet(Bullet bullet)
    {
        color = bullet.color;
        OnHit();
        //StartCoroutine(SlowTime());
        hasCollidedWithObject = true;
    }

    //TODO: In an asteroid-asteroid collision, this function is being called from both asteroids, and should just be the one getting hit.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>() is Asteroid)
        {
            Asteroid asteroidHit = collision.gameObject.GetComponent<Asteroid>();
            asteroidHit.impactVelocity = impactVelocity;
            color = asteroidHit.color;
            asteroidHit.OnHit();
        }
        else if (collision.gameObject.GetComponent<PShip>() is PShip) // TODO: These should take advantage of ShipBase polymorphism.
        {
            Debug.LogError("PLAYER COLLISION");
            PShip _player = collision.gameObject.GetComponent<PShip>();
            state = AsteroidState.Enemy;
            color = _player.renderer.material.color;
            rend.material.SetColor("_Color", color);
        }
        else if (collision.gameObject.GetComponent<EShip>() is EShip)
        {
            EShip _enemy = collision.gameObject.GetComponent<EShip>();
            state = AsteroidState.Enemy;
            color = _enemy.renderer.material.color;
            rend.material.SetColor("_Color", color);
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
