using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Group Finite State Machine/Action/Attack Exit Action")]

public class AttackExitAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm) {
        GroupManager _GM = fsm.groupManager;
        _GM.EnableFormation(false);
        _GM.ChangeAgentSpeed(20);
        _GM.Attack(false);

    }

   
}
