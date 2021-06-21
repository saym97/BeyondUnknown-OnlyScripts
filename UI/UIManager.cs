using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public struct UIWeapons {
    public GameObject uiWeaponModel;
    public WeaponStat weaponStats;
    public Sprite background;
}
public class UIManager : MonoBehaviour {
    // Start is called before the first frame update
    public Button weapon_Back, options_Back;
    private int currentWeapon;
    public RectTransform startScreen, weaponScreen, optionScreen;
    public CanvasGroup startScreenCanvasGroup;

    [Header("Loading Screen")]
    public GameObject LoadingScreen;
    public RectTransform loadingText;
    [Header("Start Screen")]
    public Button continueButton;
    public Button newGameButton;
    public Button optionsButton;
    public Button extraButton;
    public Button exitButton;

    [Header("Option Screen")]
    public Slider BackgroundVolumeSlider;
    public Slider SoundEffectsSlider;
    [Header("Weapon Stat Screen")]
    public RectTransform weaponStatsScreen;
    public TMPro.TextMeshProUGUI weaponName;
    public Image capacity;
    public Image range;
    public Image damage;
    public GameObject modelParentObject;
    public Button weaponStat_Back;
    [SerializeField]
    private List<UIWeapons> uiWeapons;
    public Button previousWeaponButton;
    public Button nextWeaponButton;
    public Image weaponBackground;



    void Start() {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("UI"));
        currentWeapon = 0;

        newGameButton.onClick.AddListener(() => {
            GameManager.instance.LoadGame();
            GameManager.instance.gameType = GameType.NewGame;
            LoadingScreen.SetActive(true);
            LeanTween.alphaText(loadingText, 1f, 0.7f).setFrom(0.2f).setLoopPingPong();

        });

        extraButton.onClick.AddListener(HideStartScreen);
        extraButton.onClick.AddListener(() => {
            ShowOtherScreen(weaponScreen, 0f);
            ShowWeaponStats(uiWeapons[currentWeapon]);
            //weaponList.blocksRaycasts = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(weaponStat_Back.gameObject);
        });

        weaponStat_Back.onClick.AddListener(ShowStartScreen);
        weaponStat_Back.onClick.AddListener(() => HideOtherScreen(weaponScreen, 800f));

        optionsButton.onClick.AddListener(HideStartScreen);

        optionsButton.onClick.AddListener(() => {
            ShowOtherScreen(optionScreen, 0f);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(options_Back.gameObject);
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
        options_Back.onClick.AddListener(ShowStartScreen);
        options_Back.onClick.AddListener(() => {
            HideOtherScreen(optionScreen, 800f);
            AudioManager.instance.SaveVolumes(BackgroundVolumeSlider.value , SoundEffectsSlider.value);
        });

        exitButton.onClick.AddListener(GameManager.instance.Quit);

        previousWeaponButton.onClick.AddListener(() => Next_Previous_Weapon(false));
        nextWeaponButton.onClick.AddListener(() => Next_Previous_Weapon(true));

        if (SaveSystem.FileExist("anomalies") || SaveSystem.FileExist("enemies")) {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.AddListener(() => {
                GameManager.instance.LoadGame();
                GameManager.instance.gameType = GameType.SavedGame;
                LoadingScreen.SetActive(true);
                LeanTween.alphaText(loadingText, 1f, 0.7f).setFrom(0.2f).setLoopPingPong();

            });
        }
    }




    void HideStartScreen() {
        Debug.Log("yellow");
        LeanTween.moveX(startScreen, -800, 10f * Time.deltaTime).setEaseInExpo();
        LeanTween.alphaCanvas(startScreenCanvasGroup, 0f, 15f * Time.deltaTime).setEaseOutQuint();
    }
    void ShowStartScreen() {
        //weaponList.blocksRaycasts = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);

        Debug.Log("yellow");
        LeanTween.moveX(startScreen, 0f, 10f * Time.deltaTime).setEaseOutExpo();
        LeanTween.alphaCanvas(startScreenCanvasGroup, 1f, 15f * Time.deltaTime).setEaseInQuint();

    }
    void ShowOtherScreen(RectTransform screen, float to) {
        LeanTween.moveX(screen, to, 10f * Time.deltaTime).setEaseOutExpo();
        LeanTween.alphaCanvas(screen.GetComponent<CanvasGroup>(), 1f, 15f * Time.deltaTime).setEaseInQuint();

    }
    void HideOtherScreen(RectTransform screen, float to) {
        LeanTween.moveX(screen, to, 10f * Time.deltaTime).setEaseInExpo();
        LeanTween.alphaCanvas(screen.GetComponent<CanvasGroup>(), 0f, 15f * Time.deltaTime).setEaseOutQuint();

    }

    void ShowWeaponStats(UIWeapons weapon) {

        modelParentObject.SetActive(true);
        weapon.uiWeaponModel.SetActive(true);
        // weaponList.blocksRaycasts = false;
        //LeanTween.moveX(weaponScreen, -800f, 10f * Time.deltaTime).setEaseInExpo();
        //weaponName.text = weapon.weaponStats.weaponName;
        weaponBackground.sprite = weapon.background;
        LeanTween.value(gameObject, (value) => { range.fillAmount = value / 100; }, 0, weapon.weaponStats.range, 10f * Time.deltaTime);
        LeanTween.value(gameObject, (value) => { capacity.fillAmount = value / 100; }, 0, weapon.weaponStats.maxAmmo, 10f * Time.deltaTime);
        LeanTween.value(gameObject, (value) => { damage.fillAmount = value / 100; }, 0, weapon.weaponStats.damageAmount, 10f * Time.deltaTime);

    }

    void Next_Previous_Weapon(bool isNext) {

        if (isNext && currentWeapon < (uiWeapons.Count - 1)) {
            Debug.Log("Next clicked");
            uiWeapons[currentWeapon].uiWeaponModel.SetActive(false);
            currentWeapon++;
            ShowWeaponStats(uiWeapons[currentWeapon]);

        }
        else if (!isNext && currentWeapon > 0) {
            Debug.Log("Previous clicked");
            uiWeapons[currentWeapon].uiWeaponModel.SetActive(false);
            currentWeapon--;
            ShowWeaponStats(uiWeapons[currentWeapon]);
        }
    }

}
