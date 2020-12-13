using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLevel : MonoBehaviour
{
    Level level = null;

    [SerializeField] Texture2D[] maps = null;
    [SerializeField] GameObject blockManagerPrefab = null;
    [SerializeField] GameObject entityManagerPrefab = null;
    [SerializeField] GameObject utilityPrefab = null;

    Utility utilityComponent = null;
    UnityBlockManager blockManagerComponent = null;
    UnityEntityManager entityManagerComponent = null;


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

        blockManagerComponent = Instantiate(blockManagerPrefab, this.transform).GetComponent<UnityBlockManager>();
        blockManagerComponent.BuildNewLevel(maps[ScoreService.Level]);

        entityManagerComponent = Instantiate(entityManagerPrefab, this.transform).GetComponent<UnityEntityManager>();
        
        entityManagerComponent.BuildNewLevel(
            maps[ScoreService.Level], 
            blockManagerComponent.manager.chunks, 
            blockManagerComponent.unityBlockChunks, 
            blockManagerComponent.Fires);
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
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}
