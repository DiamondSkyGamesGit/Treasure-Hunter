using UnityEngine;
using System.Collections;
using System;
using SkillSystem;

public class SelectTargetButton : ActionButton {

    public Enemy myTarget;

    public SelectTargetButton():base()
    {
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        AddButtonListener();

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RemoveButtonListener();

    }

    protected override void AddButtonListener()
    {
        myButton.onClick.AddListener(() => DoAction());
    }

    protected override void RemoveButtonListener()
    {
        myButton.onClick.RemoveListener(() => DoAction());
    }

    public override void DoAction()
    {
        //hvis jeg targeter en fiende burde jeg sende med en skill som jeg angriper med tenker jeg
        //får lage skill-system da
        Player.Instance.Attack((IDamageable)myTarget);
    }

    public override void DoAction(Skill skillToUse)
    {
        throw new NotImplementedException();
    }

    public override void DoAction(ActionButton theActionButton)
    {
        throw new NotImplementedException();
    }
}
