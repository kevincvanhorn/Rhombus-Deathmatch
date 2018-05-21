using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Waiting,
    BulletTurn,
    MoveTurn    
}

public class GameManager : MonoBehaviour {

    /* Public Vars: */
    [HideInInspector]
    public PShip player;
    public bool allowInput = true;

    /* Accessor Vars: */
    public static GameManager Instance { get { return _instance; } }
    public static GameState State { get { return _instance._curGameState; } }

    /* Private Singleton Vars: */
    private UIManager UIManager;
    private static GameManager _instance = null;
    private GameState _curGameState = GameState.BulletTurn;
    private GameState[] turnOrder;
    private int curTurn = -1;
    private bool canTransition = true;
    private int transitionSemaphore = 0;

    private void Awake()
    {
        /* Enforce Singleton: */
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        /* Additional intialization: */
        player = FindObjectOfType<PShip>();
        UIManager = GetComponent<UIManager>();
        turnOrder = new GameState[] {GameState.MoveTurn, GameState.BulletTurn};
    }

    private void Start()
    {
        NextTurn();
    }

    /* Different instance each scene. */
    private void OnDestroy() { if (this == _instance) { _instance = null; } }



    public void NextTurn()
    {
        if (!canTransition)
        {
            transitionSemaphore--;
            if (transitionSemaphore <= 0) canTransition = true;
        }
        

        if (canTransition)
        {
            transitionSemaphore = 0;
            curTurn = (curTurn < turnOrder.Length - 1) ? curTurn + 1 : 0;
            _curGameState = turnOrder[curTurn];
            UIManager.ShowTurnText();
        }         
    }

    public void RequestRestrictInput()
    {
        allowInput = false;
    }
    public void RequestAllowInput()
    {
        allowInput = true;
    }

    /* When a player bullet hits an asteroid. */
    public void DelayAttackTransition()
    {
        canTransition = false;
        transitionSemaphore = 1; // When 0, can transition.
    }
}
