using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Gets a list of all active enemies
/// and displays them using ScrollableActionList
/// </summary>
public class CombatUiTargetSelection : CombatUI {

    public ScrollableActionList scrollableActionList;
    public SelectTargetButton actionButtonPrefab;
    public List<Enemy> activeEnemies = new List<Enemy>();


    void OnEnable()
    {
        Messenger.AddListener<OnCombatActiveEnemies>(OnCombatGetActiveEnemies);
        Messenger.AddListener<OnCombatUISelectedAction>(OnCombatUISelectedAction);
    }

    void OnDisable()
    {
        Messenger.RemoveListener<OnCombatActiveEnemies>(OnCombatGetActiveEnemies);
        Messenger.RemoveListener<OnCombatUISelectedAction>(OnCombatUISelectedAction);
    }

    void OnCombatUISelectedAction(OnCombatUISelectedAction action)
    {
        //just listens to the state of target selection
        switch (action.selectedAction)
        {
            case SelectedAction.SELECT_TARGET:
                CreateTargetingButtonsAndDispatch();
                break;
        }
    }

    void CreateTargetingButtonsAndDispatch()
    {
        //-- Temp variable to dispatch with messenger object
        List<ActionButton> actionButtons = new List<ActionButton>();

        //-- foreach enemy, instantiate actionButtonPrefab and set text. 
        //-- Note! Their positioning is handled by reciever of event
        //-- Note! activeEnemies is updated whenever the amount of enemies change in battle
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            SelectTargetButton s = Instantiate<SelectTargetButton>(actionButtonPrefab);
            actionButtons.Add(s);
            //-- Set the text of the Button object to name of enemy + number
            s.myButton.GetComponentInChildren<Text>().text = activeEnemies[i].EnemyName + " " + i;
            //-- Set min height of the Layout component
            s.myLayoutElement.minHeight = 60f;
            //-- A selectTarget button has a target associated with the button
            s.myTarget = activeEnemies[i];
            actionButtons.Add(s);

        }
        //-- Create Message object to dispatch to listeners
        OnCombatUIDisplayActionButtons temp = new OnCombatUIDisplayActionButtons();
        temp.actionButtons = actionButtons;
        Messenger.Dispatch(temp);
    }

    void OnCombatGetActiveEnemies(OnCombatActiveEnemies data)
    {
        //Must be recieved each time the number of enemies in battle changes
        activeEnemies = data.activeEnemies;
    }

}
