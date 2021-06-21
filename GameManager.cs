using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public enum GameState {NONE,PLAYING,PAUSED,DEATH}
public enum GameType { NewGame, SavedGame}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string UIScene;
    public string GameScene;
    private List<AsyncOperation> loading = new List<AsyncOperation>();
    private List<AsyncOperation> reloading = new List<AsyncOperation>();
    public GameState gameState;
    public GameType gameType = GameType.NewGame;

    [Header("Resetting the level references")]
    public PlayerStats playerStats;
    public List<WeaponStat> weaponStats;

    public AudioClip select;
    public AudioClip enter;
    public AudioSource uiAudioSource;
    public AudioSource BackgroundaudioSource;


    void Awake()
    {
        instance = this;
        gameState = GameState.PLAYING;
        ResetEverthing();
        SceneManager.LoadSceneAsync(UIScene,LoadSceneMode.Additive);    

    }

   public void Quit() {
        Application.Quit();
    }


    public void LoadGame() {
        loading.Add(SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Additive));
        loading.Add(SceneManager.UnloadSceneAsync(UIScene));
        StartCoroutine(Loading(loading));
    }
    public void LoadMenus() {
        SceneManager.LoadScene(UIScene, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(GameScene);
        BackgroundaudioSource.UnPause();
        gameState = GameState.PLAYING;
        Time.timeScale = 1f;
        ResetEverthing();
    }

    private IEnumerator Loading(List<AsyncOperation> loading) {
        for(int i =0; i < loading.Count; i++) {
            while (!loading[i].isDone) {
                yield return null;
            }
        }
        BackgroundaudioSource.Pause();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameScene));
        Time.timeScale = 1f;
        gameState = GameState.PLAYING;
    }

    public void Reload() {
        reloading.Add(SceneManager.UnloadSceneAsync(GameScene));
        reloading.Add(SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Additive));
        StartCoroutine(Loading(reloading));
        ResetEverthing();
    }

    public void ResetEverthing() {
        playerStats.playerHealth = 100;
        playerStats.anomaliesScanned = 0;
        foreach ( WeaponStat w in weaponStats) {
            w.bulletsInMagazine = w.magSize;
            w.currentAmmo = w.maxAmmo;
        }
    }

    public void UiEnterSound() {
        uiAudioSource.PlayOneShot(enter);
    }
    public void UiSelectSound() {
        uiAudioSource.PlayOneShot(select);

    }


}
