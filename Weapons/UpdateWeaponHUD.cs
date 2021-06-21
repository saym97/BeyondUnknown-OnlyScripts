using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWeaponHUD : MonoBehaviour {
    [SerializeField]
    private TMPro.TextMeshProUGUI bullets;
    [SerializeField]
    private TMPro.TextMeshProUGUI totalBullets;
    [SerializeField]
    public Image bulletIcon;
    [SerializeField]
    private Image crosshair;
    [SerializeField]
    private TMPro.TextMeshProUGUI anomaliesScanned;
    [SerializeField]
    private Image bloodSplatter;
    // Start is called before the first frame update


    [SerializeField]
    private Image healthbar;
    [SerializeField]
    Image healthFollowBar;
    private int maxHealth;
    [SerializeField]
    private PlayerStats playerStats;
    BeyondUnknownEventSystem eventSystem;
    bool isBloodEffect;

    public SavedIDs database;
    void Start() {
        col = bloodSplatter.color;
        maxHealth = 100;

        //METHOD SUBSCRIBING TO THE EVENT SYSTEM
        eventSystem = BeyondUnknownEventSystem.singleton;
        eventSystem.Shootdelegate += WeaponStatUI;
        eventSystem.SwitchDelegate += WeaponStatUI;
        eventSystem.ReloadDelegate += WeaponStatUI;
        eventSystem.PlayerHealthDelegate += UpdateHealthBar;
        eventSystem.AnomalyScanned += AnomalyScannedSuccessfully;

        //CALLING UPDATE HEALTH IN ORDER TO GET LATEST HEALTH AT START
        UpdateHealthBar(0, true);
        anomaliesScanned.text = (playerStats.anomaliesScanned < 10) ? "0" + playerStats.anomaliesScanned.ToString() : playerStats.anomaliesScanned.ToString();

    }

    void WeaponStatUI(Weapon weapon) {

        //IF WEAPON IS MACHETE : DISABLE CROSSHAIR, BULLETS AND MAGS
        if (weapon.weaponStat.weaponType == WeaponType.Machete) {
            bullets.text = null;
            totalBullets.text = null;
            crosshair.enabled = false;
            bulletIcon.enabled = false;
            crosshair.sprite = weapon.weaponStat.crossHair ?? null;
            return;
        }
        WeaponStat wp = weapon.weaponStat;
        //DISPLAY STATS FOR OTHER WEAPONS ON HUD
        bullets.text = (wp.bulletsInMagazine < 10) ? "0" + wp.bulletsInMagazine.ToString() : wp.bulletsInMagazine.ToString();
        totalBullets.text = (wp.currentAmmo < 10) ? "0" + wp.currentAmmo.ToString() : wp.currentAmmo.ToString();
        crosshair.enabled = true;
        bulletIcon.enabled = true;
        crosshair.sprite = wp.crossHair ?? null;
    }

    // HEALTH BAR CHANGE FUNCTION
    Color col;
    void UpdateHealthBar(int amount, bool add) {
        int newValue;
        // IF ADD TO THE HEALTH
        if (add) {

            newValue = playerStats.playerHealth + amount;
            if (newValue > 100) newValue = 100;

        }
        else {
            // IF REMOVE FROM HEALTH
            newValue = playerStats.playerHealth - amount;
            if (newValue <= 0) newValue = 0;

            //SPLATTER EFFECT MOVE IN 
            LeanTween.value(gameObject, (value) => {
                col.a = value;
                bloodSplatter.color = col;
            }, 0f, 0.27f, 1.5f).setEaseOutExpo();

            //SPLATTER EFFECT MOVE OUT
            LeanTween.value(gameObject, (value) => {
                col.a = value;
                bloodSplatter.color = col;
            }, 0.27f, 0f, 1.5f).setEaseInExpo().setDelay(1.5f);


        }
        LeanTween.value(gameObject, (value) => { healthFollowBar.fillAmount = (float)value/maxHealth; }, playerStats.playerHealth, newValue, 50f * Time.deltaTime).setEaseInOutQuad();
        healthbar.fillAmount = (float)newValue / maxHealth;
        playerStats.playerHealth = newValue;
        if (playerStats.playerHealth <= 0) GameManager.instance.gameState = GameState.DEATH;
    }

    public void AnomalyScannedSuccessfully(string id) {
        playerStats.anomaliesScanned++;
        anomaliesScanned.text = (playerStats.anomaliesScanned < 10) ? "0" + playerStats.anomaliesScanned.ToString(): playerStats.anomaliesScanned.ToString();
        database.scannedAnomalies.Add(id);
        Debug.Log(database.scannedAnomalies.Count + " ---- " + database.scannedAnomalies);
    }

    
}
