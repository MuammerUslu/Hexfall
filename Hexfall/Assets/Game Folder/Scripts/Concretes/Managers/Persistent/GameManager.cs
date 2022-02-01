using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonThisObject<GameManager>
{

    //public static bool vibrationOn;

    public static bool levelStarted;
    public static bool levelCompleted;
    public static bool levelFailed;

    public static Action OnLevelFailed;
    public static Action OnLevelCompleted;
    public static Action OnLevelStarted;

    public static GameCanvas gameCanvas;

    public enum FailReason { Bomb, NoAvailableMove }
    public FailReason failReason;
    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void FailLevel()
    {
        levelFailed = true;
    }
    private void CompleteLevel()
    {
        levelCompleted = true;
    }
    private void StartLevel()
    {
        levelStarted = true;
    }



    private void OnEnable()
    {
        OnLevelStarted += StartLevel;
        OnLevelFailed += FailLevel;
        OnLevelCompleted += CompleteLevel;
    }

    private void OnDisable()
    {
        OnLevelStarted -= StartLevel;
        OnLevelFailed -= FailLevel;
        OnLevelCompleted -= CompleteLevel;
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoNextLevel()
    {
        if (PlayerPrefs.GetInt(("CurrentLevel")) < SceneManager.sceneCountInBuildSettings - 1)
        {
            PlayerPrefs.SetInt(("CurrentLevel"), PlayerPrefs.GetInt("CurrentLevel") + 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings);
            while (rand == SceneManager.GetActiveScene().buildIndex)
            {
                Debug.Log("while");
                rand = UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings);
            }
            PlayerPrefs.SetInt(("CurrentLevel"), PlayerPrefs.GetInt("CurrentLevel") + 1);
            SceneManager.LoadScene(rand);
        }
    }

    public static bool AreWeInGamePlay()
    {
        if (levelStarted && !levelFailed && !levelCompleted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StaticValuesFalse()
    {
        levelStarted = false;
        levelCompleted = false;
        levelFailed = false;
    }
}

