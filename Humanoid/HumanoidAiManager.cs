using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class HumanoidAiManager : MonoBehaviour
{
    //public static HumanoidAiManager instance;
    public string databaseId;
    public string idNumber;
    public List<GameObject> groupAiMembers;
    //private SavedIDs database;
    public Transform player;
    public Transform[] Waypoint;
    public Transform playerFeet;
    private void Awake() {
        //database = FindObjectOfType<SavedIDs>();
        //databaseId = "Humanoid" + "-" + transform.position.sqrMagnitude + "-" + idNumber;
        
        //instance = this;
    }
    private void Start() {
        /*if (database.killedEnemies.Contains(databaseId)) {
            foreach(GameObject g in groupAiMembers) {
                Destroy(g);
            }
            Destroy(this.gameObject);
        }*/
    }
    
   /* public void StoreKilledEnemies() {
        database.killedEnemies.Add(databaseId);
    }*/
}
