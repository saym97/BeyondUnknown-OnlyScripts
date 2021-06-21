using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = " Single Finite State Machine/Action/Single Chase")]

public class Single_Chase_Action : FSMAction {
    float time = 0;
    public float delayTime;
    public float giveUpMaxTime = 5;
    Animal animal;
    float giveUpTime = 0;
    public override void Act(FiniteStateMachine fsm) {
        animal = fsm.animal;
        time += Time.deltaTime;
        if(time > delayTime ) {
            Debug.Log("Chasing");
            animal.Single_Chase();
            time = 0;
        }

        if (animal.isCalled) {
            giveUpTime += Time.deltaTime;
            if(giveUpTime > giveUpMaxTime) {
                animal.isCalled = false;
                Debug.Log("NOt called anymore");
                giveUpTime = 0;
            }
        } 
    }
}
