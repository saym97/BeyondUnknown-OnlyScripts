using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacheteMech : MonoBehaviour {

    public ParticleSystem spark;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            spark.Play();
            Debug.Log("ENEMY !!!!!!!!!!!!!!!");
        }
    }
}
