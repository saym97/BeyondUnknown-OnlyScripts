using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Condition/Can Retreat")]

public class CanRetreat : Condition {
    public override bool Test(FiniteStateMachine fsm) {
        return fsm.groupManager.CanRetreat();
    }  
}
