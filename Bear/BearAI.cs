using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;
[RequireComponent(typeof(NavMeshAgent))]
public class BearAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    NavMeshAgent agent;
    [SerializeField]
    Transform target;
    [SerializeField]
    Animator anim;


    [Header("Variables")]
    [SerializeField]
    private float health = 10f;
    [SerializeField]
    private float seeDistance = 60.0f;
    [SerializeField]
    private float seeAngle = 180f;
    [SerializeField]
    private float biteRadius = 10f;
    [SerializeField]
    private float biteDelay = 2.0f;
    [SerializeField]
    private float clawRadius = 15f;
    [SerializeField]
    private float clawDelay = 5.0f;
    [SerializeField]
    private float attackDelay = 2f;
    [SerializeField]
    private int clawDamage = 20;
    [SerializeField]
    private int biteDamage = 50;

    Vector3 xz = new Vector3(1, 0, 1);
    [SerializeField]
    public Quarry quary;
    [Header("SFX")]
    public AudioClip passive;
    public AudioClip nonPassive;
    public AudioSource audioSource;
    bool playNonPassive  = false;
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        audioSource.clip = passive;
        audioSource.Play();
    }

    [Task]
    private bool CanSeePlayer(bool negation) {
        /*Vector3 direction = target.position - transform.position;
        float angle = Vector3.Dot(transform.forward, direction);
        float distance = direction.magnitude;
        if(distance < seeDistance && angle < seeAngle) {
            if (agent.speed < 5f) agent.speed = 5f;

            return !negation;
        }
        if (agent.hasPath) agent.ResetPath();
        if (agent.speed > 3.5f) agent.speed = 3.5f;
        anim.SetBool("Run", false);

        return negation;*/

        if (quary.hasPlayerEntered) {
            if (!playNonPassive) {
                audioSource.Stop();
                audioSource.clip = nonPassive;
                audioSource.Play();
                playNonPassive = true;
            }
            return !negation;
        }
           
        return negation;
    }
    [Task]
    private void SpeedModifier() {
        if (agent.remainingDistance < 20f) agent.speed = (agent.remainingDistance / 20) * agent.speed * 0.5f;
        Task.current.Succeed();
    }
    [Task]
    public bool IsInClawRange() {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;
        if(distance < clawRadius && distance > biteRadius) {
            return true;
        }
        anim.ResetTrigger("Claw");
        return false;
    }

    [Task]
    public bool  IsInBiteRange() {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;
        if (distance < biteRadius) {
            anim.SetBool("Run", false);
            anim.ResetTrigger("Claw");
            return true;
        }
        anim.ResetTrigger("Bite");
        return false;
    }

    [Task]
    private void  ChasePlayer() {
        agent.isStopped = false;
        agent.SetDestination(target.position);
        agent.speed = 10f; 
        anim.SetBool("Run", true);
        Task.current.Succeed();


    }

    [Task]
    private bool CanAttack(float range) {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.Dot(transform.forward, direction);
        float distance = direction.magnitude;
        if (distance < range && angle < seeAngle) {
            LookAtPlayer();
            anim.SetBool("Run", false);
            return true;
        }
        anim.ResetTrigger("Bite");
        anim.ResetTrigger("Claw");
        return false;
    }

    [Task]
    private bool BiteAttack() {
        agent.speed = 0f;
        
        anim.SetTrigger("Bite");
        Debug.Log("bite");
        //Task.current.Succeed();
        return true;


    }
    [Task]
    void LookAtPlayer() {
        Vector3 direction = Vector3.Scale((target.position - transform.position),xz);
        Debug.Log("SLERP SPEED " + (agent.angularSpeed * 2 * Time.deltaTime));
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction), 3);
        
    }

    
    void CheckImpact(float range,int damage) {
        Vector3 dir = target.position - transform.position;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,dir.normalized ,out hit, range, 1 << 8)) {
            Debug.DrawRay(transform.position, dir.normalized * hit.distance,Color.red);
            if (hit.collider.gameObject.tag == "Player") {
                BeyondUnknownEventSystem.singleton.RunPlayerHealthDelegate(damage,false);
                Debug.Log("Bear is hitting player at " + range);
            }
            
        }
    }

    void BiteImpact() {
        CheckImpact(biteRadius,biteDamage);
    }
    void ClawImpact() {
        CheckImpact(clawRadius, clawDamage);
    }


    [Task]
    private bool ClawAttack() {
        anim.SetTrigger("Claw");
        Debug.Log("claw");

        //Task.current.Succeed();
        return true;

    }

    [Task]
    private bool IsHealthLessThan(float value) {
        if (health <= value) {
            return true;
        }
        return false;
    }

    public void _EnemyGetsDamage(int value) {
        if(health > 0) {
            health -= value;
        }
    }

    [Task]
    private void Dead() {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        anim.SetTrigger("Death");
        audioSource.Stop();
        Destroy(gameObject, 4f);
        //Destroy(this);
        Task.current.Succeed();

    }

    [Task]
    private void StopAgent() {
        agent.isStopped = true;
        Task.current.Succeed();
    }


}
