using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using SkillSystem;

[System.Serializable]
public abstract class ActionButton : MonoBehaviour {

    public Button myButton;
    public LayoutElement myLayoutElement;

    public enum ActionButtonType
    {
        ATTACK,
        SELECT_TARGET
    }

    public ActionButtonType actionButtonType;

    //add sprite icon
    //add text shown

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

    protected virtual OnActionButtonClick MyActionButtonClickEventData(ActionButtonType actionBtnType)
    {
        OnActionButtonClick temp = new OnActionButtonClick();
        temp.actionButtonType = actionBtnType;
        return temp;
    }

    protected virtual OnActionButtonClick MyActionButtonClickEventData(ActionButtonType actionBtnType, ITargetable target)
    {
        OnActionButtonClick temp = new OnActionButtonClick();
        temp.actionButtonType = actionBtnType;
        temp.target = target;
        return temp;
    }

    protected void PublishActionButtonClick(OnActionButtonClick actionEvent)
    {
        Messenger.Dispatch(actionEvent);
    }
    
    protected abstract void AddButtonListener();
    protected abstract void RemoveButtonListener();

    public abstract void DoAction();
    public abstract void DoAction(Skill skillToUse);
    public abstract void DoAction(ActionButton theActionButton);

}
