using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Finite State Machine/Transition")]
public class Transition : ScriptableObject
{
    [SerializeField]
    private Condition[] decision;
    [SerializeField]
    private FSMAction action;
    [SerializeField]
    private State targetState;
    // Start is called before the first frame update
   
    public bool IsTriggered(FiniteStateMachine fsm) {
        for(int i = 0; i < decision.Length; i++) {
            if (!decision[i].Test(fsm)) return false;
        }
        return true;
    }

    public FSMAction GetAction() { return action; }
    public State GetTargetState() { return targetState;}
 }
