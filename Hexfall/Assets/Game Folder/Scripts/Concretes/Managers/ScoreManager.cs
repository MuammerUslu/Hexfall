using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance  // Not Persistent Instance.
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ScoreManager).Name;
                    instance = obj.AddComponent<ScoreManager>();
                }
            }
            return instance;
        }
    }

    public int Score { get; private set; }
    public int HighScore { get; private set; }

    public static Action OnScoreChanged, OnHighScoreChanged;
    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt("HighScore");
    }

    public void IncreaseScore(int increment)
    {
        Score += increment;
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", HighScore);

            OnHighScoreChanged?.Invoke();
        }
        OnScoreChanged?.Invoke();
    }

}
