using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Single Finite State Machine/Action/Single Patrol")]

public class Single_Patrol_Action : FSMAction {
    Animal animal;
    public override void Act(FiniteStateMachine fsm) {
        animal = fsm.animal;
        if (animal.IsAtDestination()) {
            Debug.Log("setting destination");
            animal.Single_Patrol();
        }
    }
}
