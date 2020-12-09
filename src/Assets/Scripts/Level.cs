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
    public GameObject entityManagerPrefab;
    
    [SerializeField] GameObject utilityPrefab;
    [SerializeField] Texture2D[] maps = null;

    Utility utilityComponent = null;
    BlockManager blockManagerComponent = null;
    EntityManager entityManagerComponent = null;

    #endregion

    #region Unity Event Functions
    void Awake()
    {
        ScoreService.NewGame(this.maps.Length);
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

        entityManagerComponent = Instantiate(entityManagerPrefab, this.transform).GetComponent<EntityManager>();
        entityManagerComponent.BuildNewLevel(maps[ScoreService.Level], blockManagerComponent.chunks);
    }

    void OnPlayerDeath()
    {
        ScoreService.PlayerDeath();
        LoadLevel();
    }

    void OnPlayerWin()
    {
        ScoreService.PlayerWin();
        Camera.main.transform.position = new Vector3(0, 0, -1);
        LoadLevel();
    }

    void LoadLevel()
    {
        Wipe();
        Initialize();
    }

    void Wipe()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}
