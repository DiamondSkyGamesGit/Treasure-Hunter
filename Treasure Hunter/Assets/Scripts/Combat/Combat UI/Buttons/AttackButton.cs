using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using SkillSystem;

[System.Serializable]
public class AttackButton : ActionButton
{

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
        myButton.onClick.AddListener(() => DoAction());
    }

    protected override void RemoveButtonListener()
    {
        if (myButton.onClick != null)
            myButton.onClick.RemoveListener(() => DoAction());
    }


    public override void DoAction()
    {
        PlayerCombatInput.Instance.Attack();
    }

    public override void DoAction(Skill skillToUse)
    {
        throw new NotImplementedException();
    }
}
