using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Animal : MonoBehaviour {
    [Header("Universal variable for  animals")]

    public int health;
    public NavMeshAgent agent;
    public Animator anim;
    [SerializeField]
    public bool dead;
    [SerializeField]
    protected float delayAmount;
    //[HideInInspector]
    public float currentDelay;
    public bool attack;
    public float attackRange;
    public float walkspeed;
    public float runSpeed;
    protected int damageAmount;

    [HideInInspector]
    //SPIDER VARIABLE TO BE ACCESSED BY GROUP MANAGER CLASS
    public SpiderType attackStatus;
    [HideInInspector]
    public bool retreat;
    [HideInInspector]
    //GROUP MANAGER FOR GROUP AI
    public GroupManager groupManager;

    //[HideInInspector]
    public bool canCall;
    public bool giveUp;
    //[HideInInspector]
    protected LayerMask layerMask;
    protected Vector3 xz;
    public bool isCalled;
    void Start() {
        currentDelay = 0;
        dead = false;
        attack = false;
        retreat = false;
        canCall = false;
        giveUp = true;

    }


    //METHOD THAT ARE UNIVERSAL TO EVERY ANIMAL  CHASE PLAYER, GET THE DAMAGE ,  PATROL
    public void ChasePlayer(Transform player) {
        anim.SetBool("Run", true);
        anim.SetBool("Walk", false);
        agent.SetDestination(player.position);
    }

    public void Patrol(Transform waypoint) {
        //if (attack) attack = false;
        anim.SetBool("Walk", true);
        anim.SetBool("Run", false);
        agent.SetDestination(waypoint.position);
    }

    public virtual void _EnemyGetsDamage(int damagePerBullet) {
        health -= damagePerBullet;
        anim.SetTrigger("Hit");
        if (!groupManager.isAnimalShot) groupManager.isAnimalShot = true;
        if (health <= 0 && !dead) {
            dead = true;
            attack = false;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            anim.SetTrigger("Dead");
            groupManager.allAnimals.Remove(this.gameObject);
            GetComponent<AudioSource>()?.Stop();
            Destroy(this.gameObject, 6f);
            groupManager.NextAttacker();
        }
    }

    public void Attack(bool canAttack) {
        attack = canAttack;
        groupManager.isAnimalShot = false;
        anim.SetBool("Run", false);
    }


    //VIRTUAL METHODS FOR SPIDER TYPE ANIMAL
    public virtual void Retreat(Transform player) {

    }
    ///Virtual methods related to action and conditions of single entity AI
    public virtual bool SingleCanSeePlayer(float dist, float ViewAngle) {
        return true;
    }
    public virtual void Single_Patrol() {
    }

    public virtual  void Single_Chase() {
        
    }

    public virtual  void Single_Attack() {
        
    }
    public virtual void Single_LookRotation() {

    }

    public bool IsAtDestination() {
        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }

    public void Call(Animal animal, float callRange, WaitForSeconds wait) {
        anim.SetTrigger("Call");
        IdleAnim();
        agent.ResetPath();
        StartCoroutine(Call_IE(animal,callRange,wait));
    }
    IEnumerator Call_IE(Animal animal, float callRange, WaitForSeconds wait) {
        //DO THE CALLING ANIMATION HERE
        yield return wait;
        if (!animal.dead) {
            //Animal _animal;
            RaycastHit hit;
            Debug.Log("I am Calling ");
            Collider[] animalsInRange = Physics.OverlapSphere(transform.position, callRange,layerMask);
            //Debug.Log("Humanoids in Range " + animalsInRange.Length);
            if (animalsInRange != null) {
                for(int i = 0; i < animalsInRange.Length; i++) {
                   // Debug.Log(animalsInRange[i].gameObject.name + " " + animalsInRange[i].gameObject.layer);
                    Animal _animal = animalsInRange[i].GetComponent<Animal>();
                    if (_animal == this) {
                        Debug.Log(_animal.gameObject.name);
                        continue; }
                    Debug.Log(_animal.gameObject.name);

                    _animal.isCalled = true;
                }  
            }
        }
    }


    public void ISCalledAction(WaitForSeconds wait) {
        agent.ResetPath();
        IdleAnim();
        anim.SetTrigger("Call");
        StartCoroutine(IsCalled_IE(wait));
    }
    IEnumerator IsCalled_IE(WaitForSeconds wait) {
        //DO THE ANIMATION OF BEING CALLED HERE
        yield return wait;
        Debug.Log("Being Called");
        isCalled = false;
        giveUp = false;

    }


    public void WalkAnim( bool active) {
        anim.SetBool("Walk", active);
       if (active) agent.speed = walkspeed;
    }

    public void RunAnim(bool active) {
        anim.SetBool("Run", active);
        if (active) agent.speed = runSpeed;

    }

    public void IdleAnim() {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
    }

}
