using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Action/Chase Entry Action")]

public class ChangeAgentSpeedAction : FSMAction {
    public float speed;
    public override void Act(FiniteStateMachine fsm) {
        fsm.groupManager.ChangeAgentSpeed(speed);
    }
}
