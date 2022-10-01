using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("ManagerNull");
            }
            return _instance;
        }
    }

    //LightingState
    public GameState gameState;

    //Events
    public static event Action<GameState> OnGameStateChange;

    [SerializeField] GameObject levelWinUI;
    public Transform playerTransform;
    GameObject player;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (this != _instance)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.Planning:
                levelWinUI.SetActive(false);
                break;
            case GameState.Playing:
                break;
            case GameState.LevelEnd:
                levelWinUI.SetActive(true);
                break;
            default:
                Debug.LogError("Somethings wrong I can feel it");
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }    
}

public enum GameState 
{
    Planning,
    Playing,
    LevelEnd
}

