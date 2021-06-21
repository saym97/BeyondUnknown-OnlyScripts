using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public Image[] healthPackIcons;
    public int healthPacks = 3;
    public int maxHealthPacks = 3;
    bool infiniteHealthCheat;
    bool nelioMode;

    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private AudioSource health;
    [SerializeField]
    private AudioClip healthSound;

    private void Start() {
        infiniteHealthCheat = false;
        nelioMode = false;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Q) && healthPacks > 0 && playerStats.playerHealth < 100) {
            health.PlayOneShot(healthSound);
            BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(25, true);
            healthPacks--;
            EnableHealthPacks(healthPacks);
        }

        //Infinite Health cheat
        if (Input.GetKeyDown(KeyCode.F4)) {
            infiniteHealthCheat = !infiniteHealthCheat;
        }

        //Nelio mode
        if (Input.GetKeyDown(KeyCode.F5)) {
            nelioMode = !nelioMode;
        }

        if (infiniteHealthCheat || nelioMode) {
            BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(100, true);
        }
    }

    public void ReplenishHealth() {
        BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(100, true);
        healthPacks = maxHealthPacks;
        EnableHealthPacks(healthPacks);
    }

    void DisableHealthPacks() {
        foreach(Image i in healthPackIcons) {
            i.enabled = false;
        }
    } 
    public void EnableHealthPacks( int i) {
        DisableHealthPacks();
        for(int h = 0; h < i; h++) {
            healthPackIcons[h].enabled = true;
        }
    }
}
