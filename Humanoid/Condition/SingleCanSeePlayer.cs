using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = " Single Finite State Machine/Condition/SingleCanSeePlayer")]

public class SingleCanSeePlayer : Condition
{
    public float distance;
    public float angle;
    public bool negation;
    public override bool Test(FiniteStateMachine fsm) {

        if (fsm.animal.SingleCanSeePlayer((distance* distance), angle)) {
            return !negation;
        }
        return negation;
    }

}
