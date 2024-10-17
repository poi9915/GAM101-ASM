using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    private int score;
    private HUD hudController;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hudController = FindObjectOfType<HUD>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        hudController = FindObjectOfType<HUD>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    public void AddScore(int amount)
    {
        score += amount;
        hudController.UpdateScore(score);
    }

    public void ResetScore()
    {
        score = 0;
    }
    public void GameOver()
    {
        hudController.ShowDeadPopup();
        Time.timeScale = 0;
        score = 0;
    }
}