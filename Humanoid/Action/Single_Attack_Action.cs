using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Single Finite State Machine/Action/Single Attack")]

public class Single_Attack_Action : FSMAction {
    Animal animal;
    public override void Act(FiniteStateMachine fsm) {
        animal = fsm.animal;
        animal.Single_Attack();
    }
}
