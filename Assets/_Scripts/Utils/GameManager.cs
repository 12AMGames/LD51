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

    public List<Sound> sounds = new List<Sound>();

    public Transform playerTransform;
    public GameObject PauseUI;
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

    public void PauseGame()
    {
        if (PauseUI.activeSelf)
        {
            PauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            LevelLoaderManager.Instance.ResumeGame();
        }
        else
        {
            PauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            LevelLoaderManager.Instance.PauseGame();
        }
    }
}

public enum GameState 
{
    Planning,
    Playing,
    LevelEnd
}

