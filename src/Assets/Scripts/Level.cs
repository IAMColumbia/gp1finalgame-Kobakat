using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Properties
    public Vector2 LevelDimensions { get; private set; }
    public Rect goalRect { get; private set; }
    public Transform playerTransform { get; private set; }
    
    public GameObject blockManagerPrefab;
    public GameObject playerPrefab;
    
    [SerializeField] GameObject utilityPrefab;
    [SerializeField] GameObject goalPrefab;
    [SerializeField] Texture2D[] maps = null;

    Utility utilityComponent = null;
    BlockManager blockManagerComponent = null;

    Color[] colorMap;
    Color[,] placeMap;


    #endregion

    #region Unity Event Functions
    void Awake()
    {
        Initialize();
    }

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

    #endregion

    #region Logic Functions

    public void Initialize()
    {
        utilityComponent = Instantiate(utilityPrefab, this.transform).GetComponent<Utility>();
        utilityComponent.Initialize(maps[ScoreService.Level]);

        blockManagerComponent = Instantiate(blockManagerPrefab, this.transform).GetComponent<BlockManager>();
        blockManagerComponent.BuildNewLevel(maps[ScoreService.Level]);
    }

    void OnPlayerDeath()
    {
        ResetLevel();
    }

    void OnPlayerWin()
    {
        ResetLevel();
    }

    void ResetLevel()
    {
        Camera.main.GetComponent<CameraFollow>().GetTarget();
    }

    

    #endregion
}
