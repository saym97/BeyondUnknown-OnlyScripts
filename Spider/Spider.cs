using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum SpiderType { SURROUND, ATTACK }
public class Spider : Animal {
    public int biteDamageAmount;
    Transform pos;
    Vector3 player;
    public Transform spiderHead;
    public GameObject webPrefab;
    
    void Start() {
        pos = groupManager.player.transform;
        xz = new Vector3(1, 0, 1);
        damageAmount = 10;
    }

    void Update() {


        if (retreat) {
            agent.Move(transform.forward * agent.speed * Time.deltaTime);
        }
        if (!attack) return;
        currentDelay += Time.deltaTime;
        player = new Vector3(pos.localPosition.x, 0, pos.localPosition.z);
        if (attack && groupManager.IsAtDestination(this.gameObject)) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.Scale((pos.localPosition - transform.position),xz )), agent.angularSpeed * Time.deltaTime);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }
        if (attack && attackStatus == SpiderType.ATTACK) {

            float distance = Vector3.Distance(groupManager.player.transform.position, transform.position);
            if (distance < (attackRange * 2) && distance > attackRange && currentDelay > delayAmount) {
                Throw();
                currentDelay = 0;
            }
            else if (distance < attackRange && currentDelay > (delayAmount / 2)) {
                if (currentDelay < delayAmount)
                    anim.SetTrigger("Bite");
                else {
                    RaycastHit hit;
                    Vector3 direction = pos.localPosition - transform.localPosition;
                    if (Physics.Raycast(transform.localPosition,direction.normalized, out hit, attackRange, (1 << 8))) {
                        Debug.DrawRay(transform.localPosition, transform.forward * attackRange, Color.white);
                        Debug.Log(hit.collider.gameObject.name);

                        BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(damageAmount, false);
                    }
                    //Debug.DrawRay(transform.localPosition, transform.forward * attackRange, Color.white);
                    currentDelay = 0;
                }
            }
            else if (distance > 10) {
                agent.SetDestination(player);
                anim.SetBool("Run", true);
            }
        }
    }





    public override void Retreat(Transform player) {
        agent.ResetPath();
        Vector3 retreatDirection = this.transform.position - player.position;
        transform.rotation = Quaternion.LookRotation(retreatDirection);
        Destroy(gameObject, 3);

    }

    public void Bite() {

        anim.SetTrigger("Bite");
        BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(biteDamageAmount, false);

    }
    [ContextMenu("Throw Web")]
    public void Throw() {
        //Vector3 direction = groupManager.player.transform.position - this.transform.position;
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), agent.angularSpeed * Time.deltaTime);
        anim.SetTrigger("WebShoot");
        GameObject web = Instantiate(webPrefab, spiderHead.position, Quaternion.identity);
        web.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 1, 1.5f) * 10f);
        Destroy(web, 2f);
    }

}
