using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InGameUI : MonoBehaviour
{
    [Header("Pause Screen References")]
    public Button resume;
    public Button restartGame;
    public Button mainMenu;
    public Button options;
    public Button exit;
    public GameObject pause;
    public Image background;
    
    
    [Header("Death Screen Reference")]
    public Button death_restartGame;
    public Button death_mainMenu;
    public Button death_exit;
    public GameObject deathScreen;
    public float deathTime;
    public GameObject HUD;

    [Header("option screen")]
    public RectTransform optionScreen;
    public Slider BackgroundVolumeSlider;
    public Slider SoundEffectsSlider;
    public Button option_back;
    // Start is called before the first frame update
    void Start()
    {
        deathTime = 0f;
        resume.onClick.AddListener(Resume);
        restartGame.onClick.AddListener(GameManager.instance.Reload);
        mainMenu.onClick.AddListener(GameManager.instance.LoadMenus);

        options.onClick.AddListener(() => {
            ShowOtherScreen(optionScreen, 0f);
            HideOtherScreen(pause.GetComponent<RectTransform>(), -800f);
            EventSystem.current.SetSelectedGameObject(option_back.gameObject);
            var volumes = AudioManager.instance.GetVolume();
            BackgroundVolumeSlider.value = volumes.Item1;
            SoundEffectsSlider.value = volumes.Item2;

        });
        BackgroundVolumeSlider.onValueChanged.AddListener(delegate {
            AudioManager.instance.SetVolume("BackgroundVolume", BackgroundVolumeSlider.value);
        });

        SoundEffectsSlider.onValueChanged.AddListener(delegate {
            AudioManager.instance.SetVolume("SoundEffectsVolume", SoundEffectsSlider.value);
        });
        option_back.onClick.AddListener(() => {
            ShowOtherScreen(pause.GetComponent<RectTransform>(), 0f);
            HideOtherScreen(optionScreen, 800f);
            EventSystem.current.SetSelectedGameObject(resume.gameObject);
            AudioManager.instance.SaveVolumes(BackgroundVolumeSlider.value, SoundEffectsSlider.value);



        });

        exit.onClick.AddListener(GameManager.instance.Quit);

        death_restartGame.onClick.AddListener(GameManager.instance.Reload);
        death_mainMenu.onClick.AddListener(GameManager.instance.LoadMenus);
        death_exit.onClick.AddListener(GameManager.instance.Quit);



    }


    void Resume() {
        
        pause.SetActive(false);
        HUD.SetActive(true);
        background.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        GameManager.instance.gameState = GameState.PLAYING;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(GameManager.instance.gameState == GameState.PLAYING) {
                HUD.SetActive(false);
                Time.timeScale = 0;
                pause.SetActive(true);
                background.enabled = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameManager.instance.gameState = GameState.PAUSED;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resume.gameObject);
            }else if (GameManager.instance.gameState == GameState.PAUSED) {
                Resume();
            }
            

        }

        if(GameManager.instance.gameState == GameState.DEATH) {
            
            deathTime += Time.unscaledDeltaTime;
            if (deathTime < 1) return;
            Time.timeScale = 0;
            if (deathTime < 2) return;
            HUD.SetActive(false);
            
            deathScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.instance.ResetEverthing();
            EventSystem.current.SetSelectedGameObject(death_restartGame.gameObject);
            GameManager.instance.gameState = GameState.NONE;

        }
    }


    void ShowOtherScreen(RectTransform screen, float to) {
        LeanTween.moveX(screen, to, 0.5f).setEaseOutExpo().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(screen.GetComponent<CanvasGroup>(), 1f, 0.5f).setEaseInQuint().setIgnoreTimeScale(true);

    }
    void HideOtherScreen(RectTransform screen, float to) {
        LeanTween.moveX(screen, to, 0.5f).setEaseInExpo().setIgnoreTimeScale(true);
        LeanTween.alphaCanvas(screen.GetComponent<CanvasGroup>(), 0f, 0.5f).setEaseOutQuint().setIgnoreTimeScale(true);

    }
}
