using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Two Types of input: 
 * 1. Separate Touch: User swipes first direction and bullet launches in that direction,
 *                    User swipes second direction before bullet finishes path, and bullet launches in second direction.
 * 2. Same Touch: User swipes first then second direction: bullet follows curved path. */
public class PInputController : MonoBehaviour {

    private PShip player = null;

    private Vector2 touchOrigin = -Vector2.one; // The initial location of a User touch input. (set to a dummy value)
    Vector2 touchEnd = Vector2.one;

    //private Vector2 swipeDirection;

    private void Start()
    {
        player = GetComponent<PShip>();
    }

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        /*Mouse Input: */
        if (Input.GetMouseButtonDown(0))
        {
            if (IsOriginValid(Input.mousePosition))
                touchOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && touchOrigin.x != -1)
        {
            touchEnd = Input.mousePosition;
            RequestPlayerMove(touchEnd - touchOrigin);
            touchOrigin.x = -1;
        }

#else
        /* Mobile Input: */
        if (Input.touchCount > 0)
        {
            Touch touchCur = Input.touches[0];
            // Check if this is the beginning of a touch:
            if (touchCur.phase == TouchPhase.Began)
            {
                if(isOriginValid(touchCur.position))
                    touchOrigin = touchCur.position;
            }
            // Check if touch has ended, starting from inside the screen boundary:
            else if(touchCur.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                touchEnd = touchCur.position;
                requestPlayerMove(touchEnd - touchOrigin);
                touchOrigin.x = -1;
            }
        }
#endif
    }

    /* Return true if the position is within the bounds of the Player. */
    private bool IsOriginValid(Vector3 position)
    {
        position = Camera.main.ScreenToWorldPoint(position);

        Debug.Log(position + " " + player.bounds);
        if (player != null)
        {
            position.z = player.bounds.center.z;
            return player.bounds.Contains(position);
        }
        return false;
    }

    private void RequestPlayerMove(Vector2 swipeDirection)
    {

        if (player != null && swipeDirection != Vector2.zero)
        {
            player.OnSimpleSwipe(swipeDirection);
        }
    }
}
