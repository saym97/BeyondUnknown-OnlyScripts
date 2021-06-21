using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Finite State Machine/Action")]

public abstract class FSMAction : ScriptableObject
{
    public abstract void Act(FiniteStateMachine fsm);
}
