using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeyondUnknownEventSystem : MonoBehaviour {
    public static BeyondUnknownEventSystem singleton;

    // WEAPON EVENTS
    public event Action<Weapon> Shootdelegate;
    public event Action<Weapon> ReloadDelegate;
    public event Action<Weapon> SwitchDelegate;
    public event Action<int,bool> PlayerHealthDelegate;
    public event System.Action<string> AnomalyScanned;
    public event System.Action Save;
    public GameObject player;
    public SavedIDs savedIDs;
    void Awake() {
        singleton = this;
        singleton.Save += savedIDs.SaveProgress;
    }

    public void RunshootDelegate(Weapon weapon) {
        if (Shootdelegate != null) Shootdelegate(weapon);
    }

    public void RunReloadDelegate(Weapon weapon) {
        if (ReloadDelegate != null) ReloadDelegate(weapon);
    }

    public void RunSwitchDelegate(Weapon weapon) {
        if (SwitchDelegate != null) {
            SwitchDelegate(weapon);
        }
    }

    public void RunPlayerHealthDelegate(int health, bool add) {
        if (PlayerHealthDelegate != null) {
            PlayerHealthDelegate(health, add);
        }
    }

    public void AnomalyScannedSuccessfully(string id) {
        if(AnomalyScanned != null) {
            AnomalyScanned(id);
        }
    }

    public void SaveProgress() {
        Save?.Invoke();
    }

    public static BeyondUnknownEventSystem GetInstance() {
        return singleton;
    }
}
