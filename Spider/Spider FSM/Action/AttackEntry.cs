using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Action/Attack entry Action")]
public class AttackEntry : FSMAction
{
    GroupManager groupManager;
    public override void Act(FiniteStateMachine fsm) {
         groupManager = fsm.groupManager;
        groupManager.EnableFormation(true);
        groupManager.SetDestinationOfSurrounders();
    }

    
}
