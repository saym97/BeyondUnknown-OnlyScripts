using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Action/Chase")]

public class ChaseAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm) {
        fsm.groupManager.Chase();
    }

    
}
