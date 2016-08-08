using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using SkillSystem;

public class AttackButton : ActionButton//, //IActionButtonEventPublisher
{

    //public override event OnActionButtonClick onActionButtonClick;

    void Start()
    {

    }

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

        myButton.onClick.AddListener(() => PublishActionButtonClick(MyActionButtonClickEventData(ActionButtonType.SKILL_SELECTOR_DIRECT_ACTION, mySkill)));
    }

    protected override void RemoveButtonListener()
    {
        if (myButton.onClick != null)
            myButton.onClick.RemoveListener(() => PublishActionButtonClick(MyActionButtonClickEventData(ActionButtonType.SKILL_SELECTOR_DIRECT_ACTION, mySkill)));
    }

    //might not need DoActions now....

    public override void DoAction()
    {

    }

    public override void DoAction(Skill skillToUse)
    {
        throw new NotImplementedException();
    }

    public override void DoAction(ActionButton theActionButton)
    {
        
    }

}
