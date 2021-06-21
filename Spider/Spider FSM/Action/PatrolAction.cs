using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Action/Patrol")]

public class PatrolAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm) {
        GroupManager groupManager = fsm.groupManager;
        if (groupManager.IsAtDestination()) {
            groupManager.NextWaypoint();
        }
    }

    
}
