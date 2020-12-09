using System;
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
    
    public static float Timer { get; set; }

    public static int Level { get; set; }

    static bool isTimeOut = false;
    #endregion

    #region Unity Event Functions

    void Update() 
    {
        Countdown();
        UpdateTextUI(); 
    }
    #endregion

    #region Logic
    public static void NewGame(int MaxLevels)
    {
        Score = 0;
        Lives = startingLives;
        Timer = levelTime;
        Level = 0;

        maxLevels = MaxLevels;
    }

    void UpdateTextUI()
    {
        scoreText.text = Score.ToString();
        livesText.text = "Lives: "+ Lives.ToString();
        timeText.text = ((int)Timer).ToString();
    }

    void Countdown()
    {
        if(!isTimeOut)
        {
            Timer -= Time.deltaTime;

            if (Timer < 0)
            {
                TimesUp.Invoke();
                isTimeOut = true;
            }
        }
                         
    }

    public static void PlayerDeath()
    {
        Score = scoreOnLevelStart;
        Lives--;

        if(Lives <= 0)
        {
            NewGame(maxLevels);
        }

        isTimeOut = false;
        Timer = levelTime;
    }

    public static void PlayerWin()
    {
        Score += pointsPerExtraSecond * (int)Timer;
        scoreOnLevelStart = Score;
        Timer = levelTime;
        Level++;

        if (Level > maxLevels - 1)
            Level = 1;
    }

    public static event Action TimesUp;
    #endregion

}
