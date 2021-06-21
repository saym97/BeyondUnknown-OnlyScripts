using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : Animal {
    public HumanoidAiManager manager;
    public AudioSource audioSource;
    public AudioClip passive;
    public AudioClip nonPassive;
    bool IsPlayingPassive = true;
    bool IsPlayingNonPassive = false;

    private void Awake() {
        manager.groupAiMembers.Add(this.gameObject);
        audioSource.clip = passive;
        audioSource.Play();
    }
    void Start() {
        // manager = HumanoidAiManager.instance;
        layerMask = 1 << 12;
        Debug.Log(this.gameObject.layer);
        giveUp = true;
        damageAmount = 10;
        xz = new Vector3(1, 0, 1);
        currentDelay = 2;
    }
    public void Call() {
        //DO THE CALL LOGIC
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireSphere(this.transform.position, 60);
    }

    public override void Single_Patrol() {
        if (dead) return;
        int index = Random.Range(0, manager.Waypoint.Length);
        agent.SetDestination(manager.Waypoint[index].position);

        if (!IsPlayingPassive) {
            IsPlayingPassive = true;
            IsPlayingNonPassive = false;
            audioSource.Stop();
            audioSource.clip = passive;
            audioSource.Play();
        }
        

    }

    public override void Single_Chase() {
        //if (canCall || isCalled) return;
        if (dead) return;
        if (agent.isStopped) return;
        Debug.Log("Humanoid Chase Logic Called");
        transform.LookAt(manager.playerFeet.position);
        agent.SetDestination(manager.player.position);
        if (IsPlayingNonPassive) return;
        IsPlayingNonPassive = true;
        IsPlayingPassive = false;
        audioSource.Stop();
        audioSource.clip = passive;
        audioSource.Play();
    }

    public override void Single_Attack() {
        if (dead) return;
        currentDelay += Time.deltaTime;
        //if (currentDelay > (delayAmount / 2)) {
        if (currentDelay > delayAmount) {
            anim.SetTrigger("Attack");
            currentDelay = 0;
        }
            
       // if(Vector3.Distance(transform.position) < )

        
    }


    //}
    //IdleAnim();

    //if (canCall) canCall = false;
    //if (isCalled) isCalled = false;
    //}
    public void CheckAndDealDamage() {
        RaycastHit hit;
        Vector3 direction = manager.player.localPosition - transform.localPosition;
        if (Physics.Raycast(transform.localPosition, direction.normalized, out hit, attackRange, (1 << 8))) {
            Debug.DrawRay(transform.localPosition, direction.normalized * hit.distance, Color.green);
            Debug.Log(hit.collider.gameObject.name);

            BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(damageAmount, false);
        }
    }
    Vector3 direction;
    float angle;
    RaycastHit scanHit;
    public LayerMask LayerMask = (1<< 9 | 1 << 12);
    public override bool SingleCanSeePlayer(float dist, float ViewAngle) {
        if (!manager) return false;
        direction = manager.player.localPosition - transform.localPosition;
        angle = Vector3.Dot(transform.forward, direction);
        if (direction.sqrMagnitude < (dist) && angle < ViewAngle) {
            if (Physics.Raycast(transform.localPosition, direction.normalized, out scanHit, dist,~LayerMask)) {
                Debug.DrawRay(transform.localPosition, direction.normalized * scanHit.distance, Color.blue);
                if (scanHit.collider.gameObject.tag == "Player") {
                    return true;
                }
            }
        }
        return false;
    }
    public override void _EnemyGetsDamage(int damagePerBullet) {
        health -= damagePerBullet;
        anim.SetTrigger("Hit");
        if (!canCall) canCall = true;
        if (health <= 0 && !dead) {
            dead = true;
            attack = false;
            manager.groupAiMembers.Remove(gameObject);
            agent.ResetPath();
            gameObject.GetComponent<Collider>().enabled = false;
            agent.enabled = false;
            agent.velocity = Vector3.zero;
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Dead");
            /*if (manager.groupAiMembers.Count < 1) {
                manager.StoreKilledEnemies();
                Destroy(manager.gameObject);
            }*/
            audioSource.Stop();
            Destroy(this.gameObject, 6f);
        }
    }
    public override void Single_LookRotation() {
        transform.LookAt(manager.playerFeet.position);
    }
}
