using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public enum SState { PATROL, CHASE, RETREAT }
public enum AIType {SPIDER,DOG}
public class GroupManager : MonoBehaviour {
    [SerializeField]
    private GameObject animalPrefab;
    public AIType AIType;
    public Terrain terrain;
    public int numberOfAnimals;
    public List<GameObject> allAnimals;
    public Transform[] waypoint;
    public int spawnBox;
    
    public GameObject player;
    public bool isAnimalShot;
    public FormationRadiusCondition playerInFormationRadius;
    public GameObject[] formationPoints;
    public GameObject formationobject;
    public LayerMask enemyLayer;

    bool instantiated = false;
    
    private void Awake() {
        isAnimalShot = false;
        enemyLayer = ~enemyLayer;
        
    }
    void Start() {
        Invoke("InstantiateAnimal",1);
        //player = BeyondUnknownEventSystem.singleton.player;
        //InstantiateAnimal();


    }
    //
    void InstantiateAnimal() {
        for (int i = 0; i < numberOfAnimals; i++) {
            Vector3 spawnPos = this.transform.position + new Vector3(Random.Range(-spawnBox, spawnBox),
                                                                    this.transform.position.y,
                                                                     Random.Range(-spawnBox, spawnBox));
            GameObject g = Instantiate(animalPrefab, spawnPos, Quaternion.identity);
            Animal animal = g.GetComponent<Animal>();
            animal.groupManager = this;
            allAnimals.Add(g);
            if (AIType != AIType.SPIDER) continue;
            if (i < 4) {
                animal.attackStatus = SpiderType.ATTACK;
                animal.agent.stoppingDistance = 10;
            }
            else {
                animal.attackStatus = SpiderType.SURROUND;
            }
            
        }
        if (AIType == AIType.SPIDER) GenerateFormationPoints();
        instantiated = true;
    }


    public void NextWaypoint() {
        // int index = (int)Mathf.PerlinNoise(0, 4);
        int index = Random.Range(0, 4);
        foreach (GameObject agent in allAnimals) {
            agent.GetComponent<Animal>().Patrol(waypoint[index]);
        }
    }

    public void Chase() {
        foreach (GameObject agent in allAnimals) {
            agent.GetComponent<Animal>().ChasePlayer(player.transform);
        }
    }

    float distance;
    Vector3 direction;
    float angle;
    public bool CanSeePlayer(float dist, float theta) {
        foreach (GameObject a in allAnimals) {
            distance = Vector3.Distance(player.transform.position, a.transform.position);
            direction = player.transform.position - a.transform.position;
            angle = Vector3.Angle(a.transform.forward, direction);
            if (distance < dist && angle < theta) {
                RaycastHit hit;
                if (Physics.Raycast(a.transform.position, direction.normalized, out hit, dist,enemyLayer)) {
                    Debug.DrawRay(a.transform.position, direction.normalized * hit.distance, Color.blue);
                    if (hit.collider.gameObject.tag == "Player") {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsAtDestination() {
        NavMeshAgent agent;
        foreach (GameObject animal in allAnimals) {
             agent = animal.GetComponent<NavMeshAgent>();
            if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsAtDestination(GameObject animal) {
        NavMeshAgent agent = animal.GetComponent<NavMeshAgent>();
        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }

    public void GroupRetreat() {
        foreach (GameObject agent in allAnimals) {
            var spider = agent.GetComponent<Animal>();
            spider.Retreat(player.transform);
            spider.attack = false;
            spider.retreat = true;
        }
        allAnimals.Clear();
        Destroy(this.gameObject, 5);
    }
    public bool CanRetreat() {
        return ( instantiated && allAnimals.Count < 4) ? true : false;
    }

    public void GenerateFormationPoints() {
        formationPoints = new GameObject[10];
        for (int i = 0; i < formationPoints.Length; i++) {
            formationPoints[i] = Instantiate(formationobject, transform.position, Quaternion.identity);
            formationPoints[i].transform.parent = transform;
            formationPoints[i].SetActive(false);
        }
    }

    //FUNCTIONS RELATED TO ATTACK ENTRY ACTION
    public void EnableFormation(bool active) {
        for (int i = 0; i < formationPoints.Length; i++) {
            Vector3 pos = Vector3.zero;
            pos.x = 10f * Mathf.Cos((i * 36) * Mathf.Deg2Rad);
            pos.z = 10f * Mathf.Sin((i * 36) * Mathf.Deg2Rad);
            formationPoints[i].transform.position = player.transform.position + pos;
            Vector3 heightPos = formationPoints[i].transform.position;
            heightPos.y = terrain.SampleHeight(heightPos);
            formationPoints[i].transform.position = heightPos; 
            formationPoints[i].SetActive(active);
        }
        playerInFormationRadius.oldPos = player.transform.position;
    }
    public void ChangeAgentSpeed(float number) {
        foreach (GameObject a in allAnimals) {
            a.GetComponent<NavMeshAgent>().speed = number;
        }
    }
    public void SetDestinationOfSurrounders() {
        for (int i = 0; i < allAnimals.Count; i++) {
            Animal animal = allAnimals[i].GetComponent<Animal>();
            animal.attack = true;
            if (animal.attackStatus == SpiderType.SURROUND) {
                animal.agent.SetDestination(formationPoints[i].transform.position);
                animal.agent.stoppingDistance = 1;               
            }
        }
    }
    //FUNCTIONS RELATED TO ATTACK UPDATE ACTION
    public void NextAttacker() {
        foreach (GameObject a in allAnimals) {
            Animal animal = a.GetComponent<Animal>();
            if (animal.attackStatus == SpiderType.SURROUND) {
                animal.attackStatus = SpiderType.ATTACK;
                return;
            }
        }
    }


    ///DOG
    public void Attack(bool canAttack) {
        foreach(GameObject animal in allAnimals) {
            animal.GetComponent<Animal>().Attack(canAttack);
        }
    }

}
