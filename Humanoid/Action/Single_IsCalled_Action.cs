using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Single Finite State Machine/Action/is Called Action")]

public class Single_IsCalled_Action : FSMAction
{
    WaitForSeconds wait = new WaitForSeconds(1.5f);
    public override void Act(FiniteStateMachine fsm) {
        fsm.animal.ISCalledAction(wait);
    }

  
}
