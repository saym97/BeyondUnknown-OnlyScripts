using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarry : MonoBehaviour
{
    public bool hasPlayerEntered = false;
    public GameObject bear;
    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag.Equals("Player"))
            hasPlayerEntered = true;
            bear.SetActive(true);
    }
}
