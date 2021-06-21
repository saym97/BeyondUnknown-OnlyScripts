using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Selectable))]
public class ButtonHighlighted : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler, IDeselectHandler, ISelectHandler,ISubmitHandler {
   /* public Button button;
    private void Start() {
        button = GetComponent<Button>();
    }*/
    public void OnPointerEnter(PointerEventData eventData) {
        if (!EventSystem.current.alreadySelecting)
            
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData) { 
            LeanTween.scale(gameObject,Vector3.one,Time.deltaTime).setIgnoreTimeScale(true);
        //LeanTween.moveLocal(gameObject, transform.localPosition - (Vector3.right * 10), 10f * Time.deltaTime);

        this.GetComponent<Selectable>().OnPointerExit(null);
    }

    public void OnSelect(BaseEventData eventData) {
        LeanTween.scale(gameObject, Vector3.one * 1.05f, Time.deltaTime).setIgnoreTimeScale(true);
        GameManager.instance.UiSelectSound();
        //LeanTween.moveLocal(gameObject, transform.localPosition + (Vector3.right *10),10f* Time.deltaTime);
    }

    public void OnSubmit(BaseEventData eventData) {
        GameManager.instance.UiEnterSound();
    }

    public void OnPointerClick(PointerEventData eventData) {
        GameManager.instance.UiEnterSound();
    }
}