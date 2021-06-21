using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = " Single Finite State Machine/Action/Single Attack Entry Exit")]

public class Single_Attack_Entry_Exit : FSMAction {
    public bool entry;
    public bool exit;
    public override void Act(FiniteStateMachine fsm) {
        if (entry) {
            Animal animal = fsm.animal;
            animal.canCall = false;
            animal.isCalled = false;
            animal.Single_LookRotation();
            animal.WalkAnim(false);
            animal.RunAnim(false);
        }

        if (exit) {
            fsm.animal.anim.ResetTrigger("Attack");
            fsm.animal.currentDelay = 2;
        }
    }
}
