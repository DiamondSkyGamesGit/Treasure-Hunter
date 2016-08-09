using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using SkillSystem;

/// <summary>
/// An ActionButton is the View in the MVC in terms of showing the player an Action
/// The button this script is associated with is considered using a Skill or Targeting an Actor
/// </summary>
[System.Serializable]
public abstract class ActionButton : MonoBehaviour {

    public Button myButton;
    public LayoutElement myLayoutElement;
    public Skill mySkill;

    //Can this Enum just be 2 things? Target Selector and UseSkill, then i don't need to send as much stuff in Messages
    public enum ActionButtonType
    {
        //-- Skill selector direct action means - Character chooses skill and then target, with no selection menu
        //-- A Magic Skill Action Button is also a Direct Action that handles targeting directly after it is pressed

        //--Skill selector choose skill from list means - a skill must be chosen from available skills from a list
        //-- That skill Action Button is a Direct Action


        SKILL_SELECTOR_DIRECT_ACTION,
        SKILL_SELECTOR_CHOOSE_SKILL_FROM_LIST,
        TARGET_SELECTOR
    }

    public ActionButtonType actionButtonType;

    //add sprite icon
    //add text shown

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

    protected virtual OnActionButtonClick MyActionButtonClickEventData(ActionButtonType actionBtnType, Skill skillToUse)
    {
        OnActionButtonClick temp = new OnActionButtonClick();
        temp.actionButtonType = actionBtnType;
        temp.theSkill = skillToUse;
        return temp;
    }

    protected virtual OnActionButtonClick MyActionButtonClickEventData(ActionButtonType actionBtnType, ITargetable target, Skill skillToUse)
    {
        OnActionButtonClick temp = new OnActionButtonClick();
        temp.actionButtonType = actionBtnType;
        temp.target = target;
        temp.theSkill = skillToUse;
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
