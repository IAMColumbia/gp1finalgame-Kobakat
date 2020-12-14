using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLevel : MonoBehaviour
{
    [SerializeField] Texture2D[] maps = null;
    [SerializeField] GameObject blockManagerPrefab = null;
    [SerializeField] GameObject entityManagerPrefab = null;
    [SerializeField] GameObject utilityPrefab = null;

    Utility utilityComponent = null;
    UnityBlockManager blockManagerComponent = null;
    UnityEntityManager entityManagerComponent = null;

    LevelAudio music = null;

    #region Unity Event Functions
    void Awake()
    {
        ScoreService.NewGame(this.maps.Length);      
        this.music = GetComponent<LevelAudio>();
        this.music.Initialize();
        Initialize();
    }

    void OnEnable()
    {
        DyingState.PlayerDied += OnPlayerDeath;
        DyingState.PlayerPauseComplete += OnPlayerPauseComplete;
        DyingState.PlayerJustDied += OnPlayerJustDied;
        WinState.PlayerWon += OnPlayerWin;
        WinState.PlayerJustWon += OnPlayerJustWon;
    }

    void OnDisable()
    {
        DyingState.PlayerDied -= OnPlayerDeath;
        DyingState.PlayerPauseComplete -= OnPlayerPauseComplete;
        DyingState.PlayerJustDied -= OnPlayerJustDied;
        WinState.PlayerWon -= OnPlayerWin;
        WinState.PlayerJustWon -= OnPlayerJustWon;
    }

    void Update()
    {
        CheckForLevelMusicChange();
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

        music.StopClip();
        music.PlayClip(music.musicIntro);
    }

    void OnPlayerJustDied()
    {
        music.Source.loop = false;
        music.StopClip();
    }

    void OnPlayerDeath()
    {
        ScoreService.PlayerDeath();
        LoadLevel();
    }

    void OnPlayerPauseComplete()
    {       
        music.PlayClip(music.musicDeath);
    }

    void OnPlayerWin()
    {
        ScoreService.PlayerWin();
        Camera.main.transform.position = new Vector3(0, 0, -1);
        LoadLevel();
    }

    void OnPlayerJustWon()
    {
        music.StopClip();
        music.Source.loop = false;
        music.PlayClip(music.musicWin);
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

    void CheckForLevelMusicChange()
    {
        if(music.Source.clip == music.musicIntro && !music.Source.isPlaying)
        {
            music.Source.loop = true;
            music.Source.clip = music.musicScore;
            music.Source.Play();
        }
    }
    #endregion
}
