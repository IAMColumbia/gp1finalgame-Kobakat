using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreService : MonoBehaviour
{
    public static int startingLives = 4;
    public static int levelTime = 200;
    public static int pointsPerExtraSecond = 10;

    [SerializeField] Text scoreText, livesText, timeText = null;

    static int scoreOnLevelStart;
    static int maxLevels;

    #region Properties
    public static int Score { get; set; }

    public static int Lives { get; set; }
    
    public static int Time { get; set; }

    public static int Level { get; set; }

    #endregion

    #region Unity Event Functions
    void Awake() { }

    void Update() { UpdateTextUI(); }
    #endregion

    #region Logic
    public static void NewGame(int MaxLevels)
    {
        Score = 0;
        Lives = startingLives;
        Time = levelTime;
        Level = 0;

        maxLevels = MaxLevels;
    }

    void UpdateTextUI()
    {
        scoreText.text = Score.ToString();
        livesText.text = Score.ToString();
        timeText.text = Time.ToString();
    }

    public static void PlayerDeath()
    {
        Score = scoreOnLevelStart;
        Lives--;

        if(Lives <= 0)
        {
            NewGame(maxLevels);
        }
    }

    public static void PlayerWin()
    {
        Score += pointsPerExtraSecond;
        scoreOnLevelStart = Score;

        Level++;

        if (Level > maxLevels - 1)
            Level = 1;
    }

    #endregion

}
