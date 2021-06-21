using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dog : Animal {
    Transform pos;
    public AudioSource audioSource;
    public AudioClip dogAttack;
    private void Start() {
        pos = groupManager.player.transform;
        damageAmount = 10;
        xz = new Vector3(1, 0, 1);

    }
    private void Update() {
        if (attack) {

            currentDelay += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.Scale((pos.localPosition - transform.position), xz)), agent.angularSpeed * Time.deltaTime);
            Vector3 player = new Vector3(groupManager.player.transform.position.x, 0, groupManager.player.transform.position.z);
            float distance = Vector3.Distance(groupManager.player.transform.position, transform.localPosition);
            Debug.Log(distance);
            if (distance < attackRange && currentDelay > delayAmount) {
                currentDelay = 0;
                /* if (currentDelay < delayAmount)
                     anim.SetTrigger("Bite");
                     audioSource.PlayOneShot(dogAttack);
                 else {*/

                RaycastHit hit;
                Vector3 direction = pos.localPosition - transform.localPosition;
                Debug.DrawRay(transform.localPosition, transform.forward * attackRange, Color.white);
                if (Physics.Raycast(transform.localPosition, direction.normalized, out hit, attackRange, (1 << 8))) {
                    Debug.DrawRay(transform.localPosition, transform.forward * attackRange, Color.white);
                    Debug.Log(hit.collider.gameObject.name);
                    audioSource.PlayOneShot(dogAttack);
                    BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(damageAmount, false);
                }
                //Debug.DrawRay(transform.localPosition, transform.forward * attackRange, Color.white);
                //}
            }
        }


    }
}
