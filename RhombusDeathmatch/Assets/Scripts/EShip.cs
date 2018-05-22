using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EShip : ShipBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
    public void LaunchBulletSingle(Vector2 swipeDirection)
    {
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
