using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCache : MonoBehaviour {

    public bool playerInRange = false;
    public GameObject prompt;
    public Shooting weapons;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip ammoPickUpSound;

    private void Update() {
        if (playerInRange && Input.GetKey(KeyCode.E)) {
            AmmoPickUp();
        } 
    }

    void AmmoPickUp() {
        source.PlayOneShot(ammoPickUpSound);
        for (int i = 0; i < weapons.weaponCollection.Count; i++) {
            WeaponStat weaponStat = weapons.weaponCollection[i].weaponStat;
            weaponStat.currentAmmo += Mathf.FloorToInt(weaponStat.maxAmmo * 0.15f);
            Debug.Log("Ammo to be added" + Mathf.FloorToInt(weaponStat.maxAmmo * 0.15f));
            if(weaponStat.currentAmmo > weaponStat.maxAmmo) {
                weaponStat.currentAmmo = weaponStat.maxAmmo;
              //  BeyondUnknownEventSystem.singleton.RunshootDelegate(weapons.weaponCollection[i]);
            }
        }
        BeyondUnknownEventSystem.singleton.RunshootDelegate(weapons.weaponCollection[weapons.currentWeapon]);
        prompt.SetActive(false);
        Destroy(gameObject);
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
