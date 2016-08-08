using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class CombatUIDefaultActions : CombatUI {

    void OnEnable()
    {
        Messenger.AddListener<OnCombatUISelectedAction>(OnCombatUISelectedAction);
    }

    void OnDisable()
    {
        Messenger.RemoveListener<OnCombatUISelectedAction>(OnCombatUISelectedAction);
    }

    void OnCombatUISelectedAction(OnCombatUISelectedAction action)
    {
        switch (action.selectedAction)
        {
            case SelectedAction.NOT_SELECTED_YET_DISPLAY_DEFAULT_ACTIONS:
                CreateActionButtonsAndDispatch();
                break;
        }
    }

    void CreateActionButtonsAndDispatch()
    {
        var v = new List<ActionButton>();
        for (int i = 0; i < actionButtonsPrefabs.Count; i++)
        {
            //-- Instantiate clones of prefabs in parent's list of ActionButtons
            ActionButton a = Instantiate(actionButtonsPrefabs[i]) as ActionButton;
            //-- Add to actionButton list in parent to dispatch in object
            v.Add(a);
        }

        //-- Create Message objects
        OnCombatUIDisplayActionButtons temp = new OnCombatUIDisplayActionButtons();
        temp.actionButtons = v;
        Messenger.Dispatch(temp);
    }

}
