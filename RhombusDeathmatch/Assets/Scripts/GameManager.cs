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

    /* Accessor Vars: */
    public static GameManager Instance { get { return _instance; } }
    public static GameState State { get { return _instance._curGameState; } }

    /* Private Singleton Vars: */
    private UIManager UIManager;
    private static GameManager _instance = null;
    private GameState _curGameState = GameState.BulletTurn;
    private GameState[] turnOrder;
    private int curTurn = -1;

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
        curTurn = (curTurn < turnOrder.Length) ? curTurn + 1 : 0;
        _curGameState = turnOrder[curTurn];
        UIManager.ShowTurnText();
    }
}
