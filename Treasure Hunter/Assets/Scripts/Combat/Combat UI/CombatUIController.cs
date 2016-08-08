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
    
    public ScrollableActionList scrollableActionList;
    public Text activeHeroNameTextObj;

    //public CombatUIDefaultActions defaultActions;

    //the sorted list on who has highest actionBarvalue
    public List<Hero> activeHeroesSorted = new List<Hero>();

    //used to let the player scroll through the list as a round-robin using Input.GetAxis from PlayerCombatInput
    int activeHeroesSortedIndex = 0;

    //might not be a good system
    public Hero activeHero;

    public SelectedAction previousSelectedAction;
    public SelectedAction selectedAction;

    public bool showDebugLogs = false;


    #region --//-- Monobehaviour Methods --\\--

    void Start () {
	
        
	}

    void OnEnable()
    {
        //FIX THIS LATER
        if (activeHero == null)
            activeHero = GameController.Instance.activeHeroes[0];

        Messenger.AddListener<OnBattleStateChanged>(OnBattleStateChanged);
        Messenger.AddListener<OnActionButtonClick>(OnActionButtonClicked);
        Messenger.AddListener<OnCombatUISelectedAction>(OnSelectedAction);
        Messenger.AddListener<OnCombatUIChangeActiveHero>(SetActiveHeroByInputDirection);
        //add listener to OnActiveHeroChanged as well


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

    public void OnBattleStateChanged(OnBattleStateChanged newBattleState)
    {
        switch (newBattleState.currentBattleState)
        {
            case (BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):

                //-- Get sorted hero list, pos[0] should be one with highest actionBarValue --
                activeHeroesSorted = GetSortedHeroListByActionBarValue(CombatController.Instance.activeHeroes);
                activeHero = activeHeroesSorted[0];

                if (showDebugLogs) { 
                    foreach (var v in activeHeroesSorted)
                        Debug.Log(v.heroName + " " + v.MyActionBar.CurrentValue);
                }
                //-- Display scrollableActionList --
                EnablePlayerInputUI(activeHero);

                break;

            case (BattleState.NORMAL_TIME_FLOW):

                DisablePlayerInputUI();

                break;
        }
    }

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

    void SetActiveHeroPortraitData()
    {
        activeHeroNameTextObj.text = activeHero.heroName;
    }

    /// <summary>
    /// Sort incoming list by highest actionBarValue as first
    /// </summary>
    private List<Hero> GetSortedHeroListByActionBarValue(List<Hero> heroes)
    {
        //don't need to sort, just orderBy
        //heroes.Sort(delegate (Hero a, Hero b) { return (a.actionBar).CompareTo(b.actionBar); });
        heroes = heroes.OrderByDescending(x => x.MyActionBar.CurrentValue).ToList();
        //List<Hero> temp = new List<Hero>();
       // temp = heroes;
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

        //-- Set textObject to represent HeroName
        activeHeroNameTextObj.text = theHero.heroName;

        //-- Get available actions from Hero and display accordingly in ScrollableActionList?
        SetCurrentSelectedAction(SelectedAction.NOT_SELECTED_YET_DISPLAY_DEFAULT_ACTIONS);

    }

    public void DisablePlayerInputUI()
    {
        scrollableActionList.DestroyActionButtons();
        scrollableActionList.gameObject.SetActive(false);
    }

    #endregion

    public void OnActionButtonClicked(OnActionButtonClick onActionBtnClicked)
    {
        Debug.Log("Got tha message! Button clicked was of ButtonType " + onActionBtnClicked.actionButtonType);
        if (onActionBtnClicked.target != null) Debug.Log("The Target was of type " + onActionBtnClicked.target);

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
                        SetCurrentSelectedAction(SelectedAction.SELECT_TARGET);
                    }
                    else
                        Debug.LogWarning("The button you pressed had no Skill associated!");

                break;
            
            //-- If the button is SkillSelector - choose a skill then display the possible Skills to use based on activeHero in GUI
            case ActionButton.ActionButtonType.SKILL_SELECTOR_CHOOSE_SKILL_FROM_LIST:

                break;

            //-- If the button is a Target Selector, Dispatch the target with the Skill to use
            case ActionButton.ActionButtonType.TARGET_SELECTOR:
                if(onActionBtnClicked.target != null) {

                    //-- Check which targetType the ITargetable was, dispatch events accordingly
                    DispatchSelectedTargetByTargetType(onActionBtnClicked.target.targetType);
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
