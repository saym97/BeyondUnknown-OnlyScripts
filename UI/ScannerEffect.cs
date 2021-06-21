using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerEffect : MonoBehaviour
{
    [SerializeField]
    bool IsScannerSpiking;
    [SerializeField]
    ParticleSystem scanner;
    [SerializeField]
    GameObject player;
    ParticleSystem.EmissionModule emission;
    
    // Start is called before the first frame update
    void Start()
    {
        IsScannerSpiking = false;
        emission = scanner.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsScannerSpiking) return;
        emission.rateOverTime = (int) 20/Vector3.Distance(player.transform.localPosition,transform.position);


    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
            IsScannerSpiking = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == player) {
            ResetScanner();
        }
    }
    private void ResetScanner() {
        IsScannerSpiking = false;
        emission.rateOverTime = 0.5f;
    }
    //public void OnDestroy() {
     //   ResetScanner();
    //}
}
