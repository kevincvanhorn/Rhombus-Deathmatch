using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Two Types of input: 
 * 1. Separate Touch: User swipes first direction and bullet launches in that direction,
 *                    User swipes second direction before bullet finishes path, and bullet launches in second direction.
 * 2. Same Touch: User swipes first then second direction: bullet follows curved path. */
public class PInputController : MonoBehaviour {

    private PShip player = null;
    private LineRenderer lineRenderer;


    private Vector2 touchOrigin = -Vector2.one; // The initial location of a User touch input. (set to a dummy value)
    Vector2 touchEnd = Vector2.one;

    //private Vector2 swipeDirection;

    private void Start()
    {
        player = GetComponent<PShip>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.Instance.allowInput) {
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
                    UpdateWebplayer();
            #else
                    UpdatePhone();
            #endif
        }
    }

    void UpdateWebplayer()
    {
        /*Mouse Input: */
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.State == GameState.MoveTurn)
            {
                if (IsOriginValid(Input.mousePosition))
                {
                    touchOrigin = Input.mousePosition;
                    DrawMoveFromPlayer(touchOrigin);
                }
            }
            else
                touchOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && touchOrigin.x != -1)
        {
            touchEnd = Input.mousePosition;
            RequestPlayerMove(touchEnd - touchOrigin, touchEnd);
            touchOrigin.x = -1;

                EndLineRender();
        }
    }

    void UpdatePhone()
    {
        /* Mobile Input: */
        if (Input.touchCount > 0)
        {
            Touch touchCur = Input.touches[0];
            // Check if this is the beginning of a touch:
            if (touchCur.phase == TouchPhase.Began)
            {
                //if(isOriginValid(touchCur.position))
                touchOrigin = touchCur.position;
            }
            // Check if touch has ended, starting from inside the screen boundary:
            else if (touchCur.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                touchEnd = touchCur.position;
                RequestPlayerMove(touchEnd - touchOrigin, touchEnd);
                touchOrigin.x = -1;
            }
        }
    }

    /* Return true if the position is within the bounds of the Player. */
    private bool IsOriginValid(Vector3 position)
    {
        position = Camera.main.ScreenToWorldPoint(position);

        if (player != null)
        {
            position.z = player.bounds.center.z;
            return player.collider.bounds.Contains(position);
        }
        return false;
    }

    private void RequestPlayerMove(Vector2 swipeDirection, Vector2 target)
    {

        if (player != null && swipeDirection != Vector2.zero)
        {
            player.OnSimpleSwipe(swipeDirection, target);
        }
    }

    /* Helper Line on Movement Input. */    
    private void DrawMoveFromPlayer(Vector2 origin)
    {
        lineRenderer.enabled = true;
        origin = Camera.main.ScreenToWorldPoint(origin); // For using mouse.
        lineRenderer.SetPosition(0, origin);
        InvokeRepeating("UpdateLineRenderer", 0, Time.deltaTime);
    }

    private void UpdateLineRenderer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(1, mousePosition);
    }

    private void EndLineRender()
    {
        CancelInvoke();
        lineRenderer.enabled = false;
    }
}
