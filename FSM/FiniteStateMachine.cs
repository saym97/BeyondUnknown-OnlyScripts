using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{
    
    public State initialState;
    public State currentState;

    [Header("Refer the group AI object here")]
    public GroupManager groupManager;

    [Header("Refer the single AI object here")]
    public Animal animal;
    
    void Start()
    {
        currentState = initialState;
        if(currentState.GetEntryAction())
        DoActions(currentState.GetEntryAction());
    }

    // Update is called once per frame
    void Update()
    {
        if (animal && animal.dead) return;
        Transition triggeredTransition = null;
        foreach(Transition t in currentState.GetTransitions()) {
            if (t.IsTriggered(this)) {
                triggeredTransition = t;
                break;
            }
        }
        List<FSMAction> actions = new List<FSMAction>();
        if (triggeredTransition) {
            State targetState = triggeredTransition.GetTargetState();
            actions.Add(currentState.GetExitAction());
            actions.Add(triggeredTransition.GetAction());
            actions.Add(targetState.GetEntryAction());
            currentState = targetState;
        }
        else {
            foreach (FSMAction a in currentState.GetActions())
                actions.Add(a);
        }
        DoActions(actions);
    }

    void DoActions(List<FSMAction> actions) {
        foreach(FSMAction a in actions) {
            if (a != null)
                a.Act(this);
        }
    }

    void DoActions(FSMAction action) {
        action.Act(this);
    }
}
