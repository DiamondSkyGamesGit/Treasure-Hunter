using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using SkillSystem;

[System.Serializable]
public abstract class ActionButton : MonoBehaviour, IUIAction, IActionButtonEventPublisher {

    public Button myButton;
    public LayoutElement myLayoutElement;

    public event OnActionButtonClick onActionButtonClick;

    //add sprite icon
    //add text shown



    public ActionButton()
    {
        Start();
    }

	// Use this for initialization
	void Start () {
        

	}

    protected virtual void OnEnable()
    {
        if (myLayoutElement == null)
            myLayoutElement = GetComponent<LayoutElement>();
    }

   protected virtual void OnDisable()
    {

    }

    protected void PublishActionButtonClick(ActionButton theActionButton)
    {
        onActionButtonClick(theActionButton);
    }

    protected abstract void AddButtonListener();
    protected abstract void RemoveButtonListener();

    public abstract void DoAction();
    public abstract void DoAction(Skill skillToUse);
    public abstract void DoAction(ActionButton theActionButton);

}
