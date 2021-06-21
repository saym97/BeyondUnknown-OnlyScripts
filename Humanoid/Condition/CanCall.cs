using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Single Finite State Machine/Condition/Can Call")]

public class CanCall : Condition
{
    public bool negation;
    public override bool Test(FiniteStateMachine fsm) {
        if (fsm.animal.canCall) return !negation;
        return negation;
    }
}
