using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreService : MonoBehaviour
{
    public int startingLives = 4;
    public int levelTime = 200;
    public int pointsPerExtraSecond = 10;

    Text scoreText, livesText, timeText;
    int scoreOnLevelStart;
    
    #region Properties
    public static int Score { get; set; }

    public static int Lives { get; set; }
    
    public static int Time { get; set; }

    public static int Level { get; set; }

    #endregion

    #region Unity Event Functions
    void Awake() { NewGame(); }
    void OnEnable() 
    {
        DyingState.PlayerDied += OnPlayerDeath;
        WinState.PlayerWon += OnPlayerWin;
    }
    void OnDisable()
    {
        DyingState.PlayerDied -= OnPlayerDeath;
        WinState.PlayerWon -= OnPlayerWin;
    }

    void Update() { UpdateTextUI(); }
    #endregion

    #region Logic
    void NewGame()
    {
        Score = 0;
        Lives = this.startingLives;
        Time = this.levelTime;
        Level = 1;
    }

    void UpdateTextUI()
    {
        scoreText.text = Score.ToString();
        livesText.text = Score.ToString();
        timeText.text = Time.ToString();
    }

    void OnPlayerDeath()
    {
        Score = scoreOnLevelStart;
        Lives--;

        if(Lives <= 0)
        {
            //Game Over
        }
    }

    void OnPlayerWin()
    {
        Score += pointsPerExtraSecond;
        scoreOnLevelStart = Score;

        Level++;
    }

    #endregion

}
