using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Group Finite State Machine/Action/DogAttack Action")]
public class DogAttackAction : FSMAction
{
    public bool attack;
    public override void Act(FiniteStateMachine fsm) {
        
        fsm.groupManager.Attack(attack);
    }

  
}
