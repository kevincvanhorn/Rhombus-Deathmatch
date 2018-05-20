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
    public static GameState state { get { return _instance._curGameState; } }

    /* Private Singleton Vars: */
    private static GameManager _instance = null;
    private GameState _curGameState = GameState.BulletTurn;

    private void Awake()
    {
        /* Enforce Singleton: */
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        /* Additional intialization: */
        player = FindObjectOfType<PShip>();
    }

    /* Different instance each scene. */
    private void OnDestroy() { if (this == _instance) { _instance = null; } }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
