using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Single Finite State Machine/Action/Call Action")]

public class Single_Call_Action : FSMAction {
    public float callRange;
    WaitForSeconds wait = new WaitForSeconds(2.0f);
    public override void Act(FiniteStateMachine fsm) {
        fsm.animal.Call(fsm.animal, callRange, wait);
    }

   
}
