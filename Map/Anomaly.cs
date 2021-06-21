using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Anomaly : MonoBehaviour {

    public Camera mainCamera;

    public PlayerMove player;
    public float scanningProgress;
    public bool playerInRange = false;
    public float scanningSpeed;
    public Image scanningProgressBar;
    public GameObject anomalyText;
    float timer;
    public string anomalyNumber;
    private string databaseId;
    public SavedIDs savedIds;
    public ParticleSystem scanner;
    public ParticleSystem.EmissionModule emm;
    [SerializeField]
    AudioClip anomalySound;
    [SerializeField]
    AudioSource source;

    private void Awake() {
        databaseId = name + "-" + transform.position.sqrMagnitude + "-" + anomalyNumber;
    }


    private void Start() {
        scanningSpeed = 0.1f;
        scanningProgress = 0f;
        timer = Time.time;
        savedIds = FindObjectOfType<SavedIDs>();
        emm = scanner.emission;
        if (savedIds.scannedAnomalies.Contains(databaseId)) {
            Debug.Log(savedIds.scannedAnomalies.Count + "blah");
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {
        if (playerInRange && player.currentState == state.scanning) {
            scanningProgressBar.fillAmount = (scanningProgress / 100f);
            if (Time.time - timer > scanningSpeed) {
                scanningProgress += 3f;
                if (scanningProgress > 100f) {
                    player.canScan = false;
                    emm.rateOverTime = 0.5f;
                    source.PlayOneShot(anomalySound);
                    anomalyText.SetActive(false);
                    Destroy(gameObject);
                    BeyondUnknownEventSystem.singleton.AnomalyScannedSuccessfully(databaseId);
                }
                timer = Time.time;
            }
        }
        else {
            scanningProgress = 0f;
        }
    }


    private void OnTriggerEnter(Collider collision) {
        Debug.Log("Player is near anomaly.");
        GameObject player = collision.gameObject;
        if (player.tag.Equals("Player") == true) {
            // this.player.canScan = true;
            playerInRange = true;
            //anomalyText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision) {
        Debug.Log("Player is no longer near anomaly.");
        GameObject player = collision.gameObject;
        if (player.tag.Equals("Player") == true) {
            this.player.canScan = false;
            playerInRange = false;
            anomalyText.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider collider) {
        if (collider.gameObject.tag.Equals("Player")) {
            Debug.Log("raycasting to anomaly");
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10f, 1 << 13)) {
                Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red);
                this.player.canScan = true;
                playerInRange = true;
                anomalyText.SetActive(true);
                return;
            }
            else {
                this.player.canScan = false;
                //playerInRange = false;
                anomalyText.SetActive(false);
            }

        }
    }
}
