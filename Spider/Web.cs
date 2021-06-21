using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    public int staminDepletionAmount; 
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision) {
        
        if (collision.gameObject.tag == "Player") {
            if (PlayerMove.stamina < staminDepletionAmount) PlayerMove.stamina = 0;
            else PlayerMove.stamina -= staminDepletionAmount;

        }
        Destroy(this.gameObject);
        
    }
}
