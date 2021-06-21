using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Condition/Animal Shot")]

public class AnimalShot : Condition
{
    public bool negation;
    public override bool Test(FiniteStateMachine fsm) {
        if (fsm.groupManager.isAnimalShot) {
            return !negation;
        }
        return negation;
    }

}
