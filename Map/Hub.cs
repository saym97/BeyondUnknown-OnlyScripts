using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour {

    public bool playerInRange = false;
    public GameObject prompt;
    public Shooting weapons;
    public Health health;
    [SerializeField]
    private PlayerStats playerStats;
    private int bonusReceived = 0;
    
    private void Update() {

        //Replenish supplies cheat
        if (Input.GetKeyDown(KeyCode.F1)) {
            ReplenishSupplies();
            health.ReplenishHealth();
        }

        //Unlock all weapons cheat
        if (Input.GetKeyDown(KeyCode.F2)) {
            weapons.weaponCollection[2].unlocked = true;
            weapons.weaponCollection[3].unlocked = true;
            weapons.weaponCollection[4].unlocked = true;
            weapons.unLockedWeapons = 5;
        }

        //Replenish supplies hub
        if (playerInRange && Input.GetKey(KeyCode.E)) {
            checkAnomalies();
            ReplenishSupplies();
            health.ReplenishHealth();
            BeyondUnknownEventSystem.singleton.SaveProgress();
        }
    }
    private void Start() {
        checkAnomalies();
    }

    void ReplenishSupplies() {
        for (int i = 0; i < weapons.weaponCollection.Count; i++) {
            if (weapons.weaponCollection[i].unlocked) {
                weapons.weaponCollection[i].weaponStat.currentAmmo = weapons.weaponCollection[i].weaponStat.maxAmmo;
            }  
        }
        BeyondUnknownEventSystem.singleton.RunshootDelegate(weapons.weaponCollection[weapons.currentWeapon]);
    }

    void checkAnomalies() {
        int anomalies = playerStats.anomaliesScanned;
        if(anomalies >= 3 && bonusReceived == 0) {
            weapons.weaponCollection[2].unlocked = true;
            weapons.unLockedWeapons = 3;
            health.maxHealthPacks = 4;
            bonusReceived++;
        } else if (anomalies >= 6 && bonusReceived == 1) {
            for (int i = 0; i < weapons.weaponCollection.Count; i++) {
                if (weapons.weaponCollection[i].unlocked) {
                    weapons.weaponCollection[i].weaponStat.maxAmmo = Mathf.RoundToInt(weapons.weaponCollection[i].weaponStat.maxAmmo * 0.1f);
                }
            }
            weapons.weaponCollection[3].unlocked = true;
            weapons.unLockedWeapons = 4;
            health.maxHealthPacks = 5;
            bonusReceived++;
        } else if (anomalies >= 9 && bonusReceived == 2) {
            for (int i = 0; i < weapons.weaponCollection.Count; i++) {
                if (weapons.weaponCollection[i].unlocked) {
                    weapons.weaponCollection[i].weaponStat.maxAmmo = Mathf.RoundToInt(weapons.weaponCollection[i].weaponStat.maxAmmo * 0.1f);
                }
            }
            weapons.weaponCollection[4].unlocked = true;
            weapons.unLockedWeapons = 5;
            health.maxHealthPacks = 6;
            bonusReceived++;
        }
    }


    private void OnTriggerEnter(Collider collision) {
        Debug.Log("Player is near anomaly.");
        GameObject player = collision.gameObject;
        if (player.tag.Equals("Player") == true) {
            playerInRange = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision) {
        Debug.Log("Player is no longer near anomaly.");
        GameObject player = collision.gameObject;
        if (player.tag.Equals("Player") == true) {
            playerInRange = false;
            prompt.SetActive(false);
        }
    }
}
