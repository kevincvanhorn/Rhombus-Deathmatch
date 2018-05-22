using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Waiting,
    BulletTurn,
    MoveTurn,
    GameOver,
    GameWin
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
    private Asteroid[] asteroids = null;

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
        asteroids = GameObject.FindObjectsOfType<Asteroid>();
    }

    private void Start()
    {
        NextTurn();
        //asteroids = GameObject.FindObjectsOfType<Asteroid>();
    }

    /* Different instance each scene. */
    private void OnDestroy() { if (this == _instance) { _instance = null; } }

    public void NextTurn()
    {
        if (_curGameState != GameState.GameOver && _curGameState != GameState.GameWin)
        {
            canTransition = true;
        }

        for (int i = 0; i < asteroids.Length; i++)
        {
            if (asteroids[i].rigidbody.velocity != Vector2.zero)
                canTransition = false;
        }

        if (canTransition)
        {
            curTurn = (curTurn < turnOrder.Length - 1) ? curTurn + 1 : 0;
            _curGameState = turnOrder[curTurn];
            UIManager.ShowTurnText();
            for (int i = 0; i < asteroids.Length; i++)
            {
                asteroids[i].OnTurnReset();
            }
            allowInput = true;
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

    private void Update()
    {
        print(allowInput);
    }

    public void OnPlayerDeath()
    {
        _curGameState = GameState.GameOver;
        UIManager.ShowTurnText();
        canTransition = false;
        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroids[i].OnTurnReset();
        }
    }

    public void OnEnemyDeath()
    {
        _curGameState = GameState.GameWin;
        UIManager.ShowTurnText();
        canTransition = false;
        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroids[i].OnTurnReset();
        }
    }
}
