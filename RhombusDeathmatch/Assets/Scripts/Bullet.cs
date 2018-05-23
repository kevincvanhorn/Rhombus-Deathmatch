using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float moveSpeed = 10;
    public float moveDist = 5;
    public Vector3 moveDirection;
    public new Rigidbody2D rigidbody;
    public Color color;

    protected Coroutine thisCoroutine = null;
    
    protected float lifeTime;
    protected Renderer rend;

    protected void Awake()
    {
        GameManager.Instance.numActiveBullets++;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    protected void Start () {
        rend = GetComponent<Renderer>();
        lifeTime = moveDist / moveSpeed; // t = d/r
        color = rend.material.color;

        thisCoroutine = StartCoroutine(DestroyViaLifeTime());
	}

    protected void Update()
    {
        //transform.Translate(transform.position *speed);
        Vector3 pos = rigidbody.position;
        float deltaX = moveSpeed * Time.deltaTime * moveDirection.normalized.x;
        float deltaY = moveSpeed * Time.deltaTime * moveDirection.normalized.y;
        rigidbody.MovePosition(new Vector3(pos.x + deltaX, pos.y + deltaY, pos.z));
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("GravityField"))
        {
            moveSpeed *= 1.8f;

        }
        else if (collision.CompareTag("Boundary"))
        {
            rend.enabled = false;
            moveSpeed = 0;
        }
    }

    protected void OnHitAsteroid()
    {
        /* Stop Asteroid in place & let trail catch up. */
        StopCoroutine(thisCoroutine);
        rend.enabled = false;
        //moveSpeed = 0;
        StartCoroutine(DestroyAfterHittingAsteroid(1f)); // delay for trail to execute.
    }

    protected IEnumerator DestroyAfterHittingAsteroid(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    protected IEnumerator DestroyViaLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    protected void OnDestroy()
    {
        StopAllCoroutines();
        GameManager.Instance.numActiveBullets--; // Can only transition to next turn when all bullets are destroyed. 
        GameManager.Instance.NextTurn();
    }
}
