using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using HeroData;


/// <summary>
/// Handles control of which UI elements to show for each instance of ScrollableActionList at a given time
/// listens to relevant combat events
/// public scripts should be added in inspector
/// notifies CombatController when player has done Action
/// OR
/// listens to combatStateChanged from CombatController event
/// When onCombatStateChanged.CombatState == PLAYERINPUT..., THEN show relevant UI for that Hero
/// 
/// </summary>
public class CombatUIController : MonoBehaviour, IActionButtonListener {
    
    public ScrollableActionList scrollableActionList;
    public Text activeHeroNameTextObj;

    public CombatUIDefaultActions defaultActions;

    //the sorted list on who has highest actionBarvalue
    public List<Hero> activeHeroesSorted = new List<Hero>();
    //if i am to use activeHero it is very important to update active hero correctly after button listeners have been assigned to the hero's buttons
    //might not be a good system
    public Hero activeHero;
    public List<ActionButton> currentActionButtons = new List<ActionButton>();

    public enum SelectedAction
    {
        NOT_SELECTED_YET,
        ATTACK,
        MAGIC,
        ITEM,
        SELECT_TARGET_ENEMY
    }

    public SelectedAction previousSelectedAction;
    public SelectedAction selectedAction;

    public delegate void OnSelectedActionChanged(SelectedAction currentAction);
    public event OnSelectedActionChanged onSelectedActionChanged;

    public bool showDebugLogs = false;


    #region --//-- Monobehaviour Methods --\\--

    void Start () {
	
        
	}

    void OnEnable()
    {
        //FIX THIS LATER
        if (activeHero == null)
            activeHero = GameController.Instance.activeHeroes[0];

        CombatController.Instance.onBattleStateChanged += OnBattleStateChanged;
        onSelectedActionChanged += OnSelectionChange;

    }
    void OnDisable()
    {
        CombatController.Instance.onBattleStateChanged -= OnBattleStateChanged;
        onSelectedActionChanged -= OnSelectionChange;

    }

    #endregion


    #region --//-- Add & Remove Listeners Methods --\\--

    
    List<ActionButton> GetDefaultActionButtons()
    {
        return defaultActions.actionButtons;
    }

    //hopefully it goes correctly to the right hero, not a problem with prefabs or shared vars or something
    void AddButtonListeners(List<ActionButton> theActionButtons)
    {
        foreach(var v in theActionButtons)
        {
            v.onActionButtonClick += OnActionButtonClicked;
        }
    }

    void RemoveButtonListeners(List<ActionButton> theActionButtons)
    {
        foreach(var v in theActionButtons)
        {
            v.onActionButtonClick -= OnActionButtonClicked;
        }
    }

    #endregion

    #region --//-- On Battle State Changed Methods --\\--

    public void OnBattleStateChanged(BattleState battleState)
    {
        switch (battleState)
        {
            case (BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):

                //-- Get sorted hero list, pos[0] should be one with highest actionBarValue --
                activeHeroesSorted = GetSortedHeroListByActionBarValue(CombatController.Instance.activeHeroes);
                activeHero = activeHeroesSorted[0];

                if (showDebugLogs) { 
                    foreach (var v in activeHeroesSorted)
                        Debug.Log(v.heroName + " " + v.MyActionBar.CurrentValue);
                }

                //-- Set currentActionButtons
                currentActionButtons = GetDefaultActionButtons();

                //-- Display scrollableActionList --
                EnablePlayerInputUI(activeHero);

                break;

            case (BattleState.NORMAL_TIME_FLOW):

                DisablePlayerInputUI();

                break;
        }
    }

    /// <summary>
    /// Sort incoming list by highest actionBarValue as first
    /// </summary>
    /// <param name="heroes"></param>
    /// <returns></returns>
    private List<Hero> GetSortedHeroListByActionBarValue(List<Hero> heroes)
    {
        //don't need to sort, just orderBy
        //heroes.Sort(delegate (Hero a, Hero b) { return (a.actionBar).CompareTo(b.actionBar); });
        heroes = heroes.OrderByDescending(x => x.MyActionBar.CurrentValue).ToList();
        List<Hero> temp = new List<Hero>();
        temp = heroes;
        return temp;
    }

    #endregion

    #region --//--  Methods Handling Which UI components to show --\\--

    public void OnSelectionChange(SelectedAction theNewSelectedAction)
    {
        switch (theNewSelectedAction)
        {
            case (SelectedAction.NOT_SELECTED_YET):

                break;

            case (SelectedAction.ATTACK):
                //remove listeners
                //destroy current actionbuttons in scrollableList
                //Display Target Selection UI
                break;
            case (SelectedAction.MAGIC):

                break;
            case (SelectedAction.ITEM):

                break;

            case (SelectedAction.SELECT_TARGET_ENEMY):

                break;
        }
    }

    public void SetCurrentSelectedAction(SelectedAction _currentSelectedAction)
    {
        previousSelectedAction = selectedAction;
        selectedAction = _currentSelectedAction;
        if(onSelectedActionChanged != null)
            onSelectedActionChanged(selectedAction);
    }
    

    public void EnablePlayerInputUI(Hero theHero)
    {
        //-- Enable ScrollableActionList --
        scrollableActionList.gameObject.SetActive(true);

        //-- Set textObject to represent HeroName
        activeHeroNameTextObj.text = theHero.heroName;

        //-- Get available actions from Hero and display accordingly in ScrollableActionList

        scrollableActionList.InstantiateAndDisplayItems(currentActionButtons);
        //add listeners to the buttons
        //CAN ONLY BE DONE AFTER INSTANTIATION OF THE FUUUUUUUUUCKING BUTTONS REMEMBER
        AddButtonListeners(scrollableActionList.actionButtons);
    }

    public void DisablePlayerInputUI()
    {
        RemoveButtonListeners(scrollableActionList.actionButtons);
        //Destroy have to be called from outside so i can handle listeners from this class
        scrollableActionList.DestroyActionButtons();
        scrollableActionList.gameObject.SetActive(false);
    }

    #endregion

    public void OnActionButtonClicked(ActionButton theButton, ITargetable theTarget)
    {
        Debug.Log("Got tha message! Button clicked was " + theButton.GetType() + " of ButtonType " + theButton.actionButtonType);
        if (theTarget != null) Debug.Log("The Target was of type " + theTarget.targetType);
        switch (theButton.actionButtonType)
        {
            case ActionButton.ActionButtonType.ATTACK:
                SetCurrentSelectedAction(SelectedAction.ATTACK);
                //display target selection
                break;

        }
        //so now we got here.. what to do?
        //should change state somewhere, since we now have chosen an Action
        //if we have pressed Attack the current Action selected is Attack
        //next Action is Select Target
    }




}
