using UnityEngine;
using System.Collections;
using System;
using SkillSystem;

public class SelectTargetButton : ActionButton//, IActionButtonEventPublisher
{

    public Enemy myTarget;

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
        myButton.onClick.AddListener(() => PublishActionButtonClick(MyActionButtonClickEventData(ActionButtonType.SELECT_TARGET)));
    }

    protected override void RemoveButtonListener()
    {
        myButton.onClick.RemoveListener(() => PublishActionButtonClick(MyActionButtonClickEventData(ActionButtonType.SELECT_TARGET)));
    }

    public override void DoAction()
    {
        //hvis jeg targeter en fiende burde jeg sende med en skill som jeg angriper med tenker jeg
        //får lage skill-system da
        /*
        if (onActionButtonClick != null)
            onActionButtonClick(this);
            */
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
