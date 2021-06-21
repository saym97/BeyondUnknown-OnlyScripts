using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Finite State Machine/Action/Walk Run Anim Trigger")]

public class WalkRunAnimTrigger : FSMAction
{
    public bool WalkActive;
    public bool RunActive;

    public override void Act(FiniteStateMachine fsm) {
        fsm.animal.WalkAnim(WalkActive);
        fsm.animal.RunAnim(RunActive);
    }
}
