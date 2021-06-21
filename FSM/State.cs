using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/State")]
public class State : ScriptableObject
{
    [SerializeField]
    private FSMAction entryAntion;
    [SerializeField]
    private FSMAction[] stateActions;
    [SerializeField]
    private FSMAction exitAction;
    [SerializeField]
    private Transition[] transitions;

    // Start is called before the first frame update

    public FSMAction[] GetActions() {
        return stateActions;
    }

    public FSMAction GetEntryAction() {
        return entryAntion;
    }

    public FSMAction GetExitAction() {
        return exitAction;
    }

    public Transition[] GetTransitions() {
        return transitions;
    }

}
