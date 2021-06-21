using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Action/Retreat")]

public class RetreatAction : FSMAction {
    public override void Act(FiniteStateMachine fsm) {
        fsm.groupManager.GroupRetreat();
    }

}
