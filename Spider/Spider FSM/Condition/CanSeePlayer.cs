using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Group Finite State Machine/Condition/Can See")]

public class CanSeePlayer : Condition {
    // Start is called before the first frame update

    [SerializeField]
    private float angle, distance;
    [SerializeField]
    private bool negation;
    public override bool Test(FiniteStateMachine fsm) {
        bool canSee =  fsm.groupManager.CanSeePlayer(distance, angle);
        if (negation) {
            return !canSee;
        }
        else {
            return canSee;
        }
    }
}
