using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float moveSpeed = 10;
    public float moveDist = 5;
    public Vector3 moveDirection;

    private float lifeTime;
    private new Rigidbody2D rigidbody;

	// Use this for initialization
	private void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        lifeTime = moveDist / moveSpeed; // t = d/r

        StartCoroutine(DestroyDelay());
	}

    private void Update()
    {
        //transform.Translate(transform.position *speed);
        Vector3 pos = rigidbody.position;
        float deltaX = moveSpeed * Time.deltaTime * moveDirection.normalized.x;
        float deltaY = moveSpeed * Time.deltaTime * moveDirection.normalized.y;
        rigidbody.MovePosition(new Vector3(pos.x + deltaX, pos.y + deltaY, pos.z));
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
