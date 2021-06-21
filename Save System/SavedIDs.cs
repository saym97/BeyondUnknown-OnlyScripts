using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedIDs : MonoBehaviour
{

    public HashSet<string> killedEnemies { get; private set; } = new HashSet<string>();
    [SerializeField]
    public HashSet<string> scannedAnomalies = new HashSet<string>();
    //private BeyondUnknownEventSystem mainEventsystem;

    private void Awake() {
        if (GameManager.instance.gameType == GameType.SavedGame) {
            Load();
        }
        
    }
    private void OnEnable() {
       // mainEventsystem = BeyondUnknownEventSystem.singleton;
       // mainEventsystem.Save += SaveProgress;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

   public void SaveProgress () {
        SaveSystem.Save(scannedAnomalies, "anomalies");
        SaveSystem.Save(killedEnemies, "enemies");
    }

    void Load() {
        Debug.Log("sdmbf");
        if (SaveSystem.FileExist("anomalies")) {
            scannedAnomalies = SaveSystem.Load<HashSet<string>>("anomalies");
            Debug.Log(scannedAnomalies.Count);
            GameManager.instance.playerStats.anomaliesScanned = scannedAnomalies.Count;
        }
        if (SaveSystem.FileExist("enemies")) { 
            killedEnemies = SaveSystem.Load<HashSet<string>>("enemies");
        }
       
    }
}
