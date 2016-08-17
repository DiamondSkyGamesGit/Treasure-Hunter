using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using HeroData;
using SkillSystem;


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
public class CombatUIController : MonoBehaviour {

    #region --//-- Fields --\\--

    //-- The object that contains the listing of buttons
    public ScrollableActionList scrollableActionList;

    //-- The portrait object that contains active hero name in the GUI
    public Text activeHeroNameTextObj;

    //the sorted list on who has highest actionBarvalue
    public List<Hero> activeHeroesSorted = new List<Hero>();

    //used to let the player scroll through the list as a round-robin using Input.GetAxis from PlayerCombatInput
    int activeHeroesSortedIndex = 0;

    //The Hero that is currently active in the GUI, as in the Hero whose actions we are choosing
    public Hero activeHero;

    //-- The selected actions state machine used in this script
    public SelectedAction previousSelectedAction;
    public SelectedAction selectedAction;

    //-- When the player has decided use of a skill, this is registered to Dispatch later when the player has completed a cycle of interaction by choosing a target
    public Skill currentSkillSelected;

    public bool showDebugLogs = false;

    #endregion

    #region --//-- Monobehaviour Methods --\\--

    void Start () {
	
        
	}

    void OnEnable()
    {
        //FIX THIS LATER
        if (activeHero == null)
            activeHero = GameController.Instance.activeHeroes[0];

        //-- Listen to change of BattleState as defined in CombatController
        Messenger.AddListener<OnBattleStateChanged>(OnBattleStateChanged);
        //-- Listen to a click of the actionButtons. We do different things based on what Type of button was pressed
        Messenger.AddListener<OnActionButtonClick>(OnActionButtonClicked);
        //-- Listen to a change in stateMachine SelectedAction. Though it is contained in this script, it is helpful to also listen to it
        Messenger.AddListener<OnCombatUISelectedAction>(OnSelectedAction);
        //-- Listen to a change in the Active Hero on the Battle Canvas. The player may cycle through which hero he wants to perform an action
        Messenger.AddListener<OnCombatUIChangeActiveHero>(SetActiveHeroByInputDirection);
        //add listener to OnActiveHeroChanged as well?


    }
    void OnDisable()
    {
        Messenger.RemoveListener<OnBattleStateChanged>(OnBattleStateChanged);
        Messenger.RemoveListener<OnActionButtonClick>(OnActionButtonClicked);
        Messenger.RemoveListener<OnCombatUISelectedAction>(OnSelectedAction);
        Messenger.RemoveListener<OnCombatUIChangeActiveHero>(SetActiveHeroByInputDirection);

    }

    #endregion


    #region --//-- On Battle State Changed Methods --\\--

    /// <summary>
    /// Handle the changes whenever CombatController changes BattleState.
    /// Handles which UI components to Show to the Player
    /// </summary>
    public void OnBattleStateChanged(OnBattleStateChanged newBattleState)
    {
        switch (newBattleState.currentBattleState)
        {
            //-- This state is triggered when the player presses X on Controller or left mouseclick
            case (BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):

                //-- Get sorted hero list, pos[0] should be one with highest actionBarValue --
                activeHeroesSorted = GetSortedHeroListByActionBarValue(CombatController.Instance.activeHeroes);
                activeHero = activeHeroesSorted[0];

                //-- Display scrollableActionList
                EnablePlayerInputUI(activeHero);

                break;
            
            //-- Normal time flow is in Combat when Action Bars are charging and we are waiting for Actors' actions or Input
            case (BattleState.NORMAL_TIME_FLOW):

                //-- Do not display ScrollableActionList
                DisablePlayerInputUI();

                break;
        }
    }

    #endregion

    #region --//-- Methods concerning Heroes and HeroData --\\--

    /// <summary>
    /// When a entity wants to change the Active Hero in the GUI we cycle through the Active Heroes based on direction
    /// This is triggered with Player Input where the Player presses a directional button right / left
    /// The activeHero in the GUI is then changed and Dispatched to listeners
    /// </summary>
    void SetActiveHeroByInputDirection(OnCombatUIChangeActiveHero data)
    {
        switch (data.btnDirectionOnInput)
        {
            case CombatUIScrollDirection.RIGHT:

                activeHeroesSortedIndex++;
                if (activeHeroesSortedIndex > activeHeroesSorted.Count - 1)
                    activeHeroesSortedIndex = 0;
                activeHero = activeHeroesSorted[activeHeroesSortedIndex];
                OnCombatUIActiveHero temp = new OnCombatUIActiveHero();
                temp.theActiveHero = activeHero;
                Messenger.Dispatch(temp);
                SetActiveHeroPortraitData();

                break;
            case CombatUIScrollDirection.LEFT:
                activeHeroesSortedIndex--;
                if (activeHeroesSortedIndex < 0)
                    activeHeroesSortedIndex = activeHeroesSorted.Count - 1;
                activeHero = activeHeroesSorted[activeHeroesSortedIndex];
                temp = new OnCombatUIActiveHero();
                temp.theActiveHero = activeHero;
                Messenger.Dispatch(temp);
                SetActiveHeroPortraitData();
                break;
        }
    }

    /// <summary>
    /// Sets the ActiveHero portrait data (name, portrait icon) in the Combat UI
    /// </summary>
    void SetActiveHeroPortraitData()
    {
        activeHeroNameTextObj.text = activeHero.heroName;
    }

    /// <summary>
    /// Sort incoming list by highest actionBarValue as first [0]
    /// </summary>
    private List<Hero> GetSortedHeroListByActionBarValue(List<Hero> heroes)
    {
        heroes = heroes.OrderByDescending(x => x.MyActionBar.CurrentValue).ToList();
        return heroes;
    }

    #endregion

    #region --//--  Methods Handling Which UI components to show --\\--

    //-- Do i need this? Think i do, even though im running 2 statemachines in the same script
    //- One handles what to do with button clicks, this handles which GUI elements to display, and is what other scripts listen to
    public void OnSelectedAction(OnCombatUISelectedAction data)
    {
        switch (data.selectedAction)
        {
            case (SelectedAction.NOT_SELECTED_YET_DISPLAY_DEFAULT_ACTIONS):

                break;

            case (SelectedAction.SELECT_TARGET):
                //well listener already has gotten this message if the stateMachine has entered here so don't need to dispatch from here
                break;

            case (SelectedAction.SELECT_SKILL_TO_USE):

                break;

            case SelectedAction.SELECTED_TARGET_FRIENDLY:

                break;
            case SelectedAction.SELECTED_TARGET_ENEMY:

                break;

            case SelectedAction.CHANGE_ACTIVE_HERO:
                //-- Should handle the Dispatch of the new active hero?
                //dont know if i need this state..
                //I don't think it should be it's own state because the event is already handled as a custom handler for this type of event
                //and for now changing hero does not change anything in the game since they share default actions
                //BUT: if they're inside another GUI than the default Actions window then..
                //in that case need to populate GUI OnActiveHeroChanged which kinda makes sense, just need to decide when that is allowed
                //in the state machine
                break;
        }
    }

    /// <summary>
    /// Which SelectedAction state should we enter?
    /// Dispatches OnCombatUISelectedAction object, containing previous and current SelectedAction
    /// </summary>
    public void SetCurrentSelectedAction(SelectedAction _currentSelectedAction)
    {
        OnCombatUISelectedAction temp = new OnCombatUISelectedAction();
        //set previous state
        previousSelectedAction = selectedAction;
        temp.previousSelectedAction = previousSelectedAction;

        //set new currentState
        selectedAction = _currentSelectedAction;
        temp.selectedAction = selectedAction;

        //Fire event
        Messenger.Dispatch(temp);
        
    }

    public void EnablePlayerInputUI(Hero theHero)
    {
        //-- Enable ScrollableActionList --
        scrollableActionList.gameObject.SetActive(true);

        //-- Should probably set this outside of this method as in, OnActiveHEroChanged should be fired whenever and recieved by this script when the script is "enabled"
        //-- Set textObject to represent HeroName
        activeHeroNameTextObj.text = theHero.heroName;

        //-- Get available actions from Hero and display accordingly in ScrollableActionList?
        SetCurrentSelectedAction(SelectedAction.NOT_SELECTED_YET_DISPLAY_DEFAULT_ACTIONS);

    }

    public void DisablePlayerInputUI()
    {
        Debug.Log("DisablePlayerInputUI in CombatUIController was called now!");
        scrollableActionList.gameObject.SetActive(false);
    }

    #endregion

    /// <summary>
    /// Handles statemachine when a ActionButton was clicked based on the type of button that was clicked
    /// </summary>
    public void OnActionButtonClicked(OnActionButtonClick onActionBtnClicked)
    {
        if (showDebugLogs)
        {
            Debug.Log("Got tha message! Button clicked was of ButtonType " + onActionBtnClicked.actionButtonType);
            if (onActionBtnClicked.target != null) Debug.Log("The Target was of type " + onActionBtnClicked.target);
        }

       

        //-- Check which buttonType was clicked and handle event dispatches accordingly
        switch (onActionBtnClicked.actionButtonType)
        {
            //-- If the buttonType is DirectAction, choose target for the given skill directly after
            case ActionButton.ActionButtonType.SKILL_SELECTOR_DIRECT_ACTION:

                    if (onActionBtnClicked.theSkill != null)
                    {
                        //Dispatch the skill used. The ActionButton itself could do it, but it's nice to keep it in One Statemachine!
                        DispatchSkillUsed(onActionBtnClicked.theSkill);
                    
                        //-- Set SelectedAction to display targeting list
                        //-- This might be redundant. I can have the UI components listen to buttonTypes that they need
                        //-- For example, CombatUITargetSelection component just needs to listen to OnActionButtonClick.actionButtonType TargetSelector
                        //-- Then SelectedAction state machine might be redundant!
                        SetCurrentSelectedAction(SelectedAction.SELECT_TARGET);

                    }
                    else
                        Debug.LogWarning("The button you pressed had no Skill associated! The Button Prefab must have a scriptableObject Skill component!");

                break;
            
            //-- If the button is SkillSelector - choose a skill then display the possible Skills to use based on activeHero in GUI
            case ActionButton.ActionButtonType.SKILL_SELECTOR_CHOOSE_SKILL_FROM_LIST:

                break;

            //-- If the button is a Target Selector, Dispatch the target with the Skill to use
            case ActionButton.ActionButtonType.TARGET_SELECTOR:
                if(onActionBtnClicked.target != null) {

                    //-- Check which targetType the ITargetable was, dispatch events accordingly
                    //-- This might be redundant. Should probably be enough to just dispatch the chosen skill and target
                    //-- A listener must implement how they want to react to Skill and Target...
                   // Not used yet:  DispatchSelectedTargetByTargetType(onActionBtnClicked.target.targetType);
                    activeHero.UseSkillOnTarget(currentSkillSelected, onActionBtnClicked.target);
                    //CombatController.Instance.OnPlayerInputPauseIPausables(false);
                    CombatController.Instance.SetCurrentBattleState(BattleState.NORMAL_TIME_FLOW);
                }
                else
                {
                    Debug.LogWarning("The action did not have a Target! All SelectTarget Actions must have a Target!!");
                }
                break;

        }
    }

    void DispatchSkillUsed(Skill theSkill)
    {
        currentSkillSelected = theSkill;
        OnCombatUISkillSelected skillChosenMessageData = new OnCombatUISkillSelected();
        skillChosenMessageData.theSkill = theSkill;
        Messenger.Dispatch(skillChosenMessageData);
    }

    void DispatchSelectedTargetByTargetType(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.ENEMY:

                break;

            case TargetType.HERO:

                break;
        }
    }


}
