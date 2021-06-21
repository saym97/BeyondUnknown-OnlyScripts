using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Single Finite State Machine/Condition/IS Called")]

public class IsCalled : Condition
{
    public bool negation;
    public override bool Test(FiniteStateMachine fsm) {
        if (fsm.animal.isCalled) return !negation;
        return negation;
    }
}
