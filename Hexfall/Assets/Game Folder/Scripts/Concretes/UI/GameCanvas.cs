using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCanvas : MonoBehaviour
{
    public GameObject startPanel, gamePlayPanel, failedPanel;
    public TextMeshProUGUI scoreText, levelFailedScoreText, levelFailedHighScoreText, levelFailReasonText;

    private void Start()
    {
        RefreshScoreText();
    }

    public void OnClick_StartButton()
    {
        GameManager.OnLevelStarted?.Invoke();
    }

    public void OnClick_RestartButton()
    {
        GameManager.Instance.RestartLevel();
    }

    public void OnClick_GoNextLevelButton()
    {
        GameManager.Instance.GoNextLevel();
    }


    #region Added to Actions
    private void LevelStarted()
    {
        startPanel.SetActive(false);
        gamePlayPanel.SetActive(true);

    }

    private void LevelFailed()
    {
        StartCoroutine(LevelFailedCor());
    }


    IEnumerator LevelFailedCor()
    {
        yield return new WaitForSeconds(1f);
        failedPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        levelFailedHighScoreText.text = "HighScore: " + ScoreManager.Instance.HighScore;
        levelFailedScoreText.text = "Score: " + ScoreManager.Instance.Score;
        if (GameManager.Instance.failReason == GameManager.FailReason.Bomb)
        {
            levelFailReasonText.text = "Don't let a bomb explode!";
        }
        else
        {
            levelFailReasonText.text = "There is no any available move!";
        }
    }



    private void RefreshScoreText()
    {
        scoreText.text = "Score: " + ScoreManager.Instance.Score.ToString();
    }


    private void OnEnable()
    {
        GameManager.OnLevelStarted += LevelStarted;
        GameManager.OnLevelFailed += LevelFailed;

        ScoreManager.OnScoreChanged += RefreshScoreText;

        //TouchScript.OnGetHexagonSelectionInput += RefreshTouchText;
    }

    private void OnDisable()
    {
        GameManager.OnLevelStarted -= LevelStarted;
        GameManager.OnLevelFailed -= LevelFailed;

        ScoreManager.OnScoreChanged -= RefreshScoreText;

        //TouchScript.OnGetHexagonSelectionInput -= RefreshTouchText;
    }
    #endregion
}
