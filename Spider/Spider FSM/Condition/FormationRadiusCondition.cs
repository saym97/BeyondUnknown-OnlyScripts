using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Condition/Player Oustside Form radius")]

public class FormationRadiusCondition : Condition {
    public Vector3 oldPos;
    public override bool Test(FiniteStateMachine fsm) {
        if (Vector3.Distance(oldPos , fsm.groupManager.player.transform.position) > 10f) {
            return true;
        }
        return false;
    }
}
