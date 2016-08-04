using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using SkillSystem;

[System.Serializable]
public class AttackButton : ActionButton//, //IActionButtonEventPublisher
{

    //public override event OnActionButtonClick onActionButtonClick;

    protected override void OnEnable()
    {
        //unfortunately, children of ActionButton must subscribe to onClick events, can't be called from base class....
        base.OnEnable();
        AddButtonListener();
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RemoveButtonListener();
    }

    //Added these for safety in the future
    protected override void AddButtonListener()
    {
        myButton.onClick.AddListener(() => PublishActionButtonClick(this));
    }

    protected override void RemoveButtonListener()
    {
        if (myButton.onClick != null)
            myButton.onClick.RemoveListener(() => PublishActionButtonClick(this));
    }

    public override void DoAction()
    {
        /*
        if(onActionButtonClick != null)
            onActionButtonClick(this);

        Debug.Log(onActionButtonClick);
        */
        //how to fix......?
        //cant seem to get listeners on this object
    }

    public override void DoAction(Skill skillToUse)
    {
        throw new NotImplementedException();
    }

    public override void DoAction(ActionButton theActionButton)
    {
        
    }

}
