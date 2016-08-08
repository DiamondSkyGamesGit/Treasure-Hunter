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
public class CombatUIController : MonoBehaviour {
    
    public ScrollableActionList scrollableActionList;
    public Text activeHeroNameTextObj;

    //public CombatUIDefaultActions defaultActions;

    //the sorted list on who has highest actionBarvalue
    public List<Hero> activeHeroesSorted = new List<Hero>();
    //if i am to use activeHero it is very important to update active hero correctly after button listeners have been assigned to the hero's buttons
    //might not be a good system
    public Hero activeHero;
    public List<ActionButton> currentActionButtons = new List<ActionButton>();


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

    }
    void OnDisable()
    {
        Messenger.RemoveListener<OnBattleStateChanged>(OnBattleStateChanged);
        Messenger.RemoveListener<OnActionButtonClick>(OnActionButtonClicked);
        Messenger.RemoveListener<OnCombatUISelectedAction>(OnSelectedAction);

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

    public void OnSelectedAction(OnCombatUISelectedAction data)
    {
        switch (data.selectedAction)
        {
            case (SelectedAction.NOT_SELECTED_YET_DISPLAY_DEFAULT_ACTIONS):

                break;

            case (SelectedAction.ATTACK):
                SetCurrentSelectedAction(SelectedAction.SELECT_TARGET);
                break;
            case (SelectedAction.MAGIC):

                break;
            case (SelectedAction.ITEM):

                break;

            case (SelectedAction.SELECT_TARGET):

                break;

            case SelectedAction.SELECTED_TARGET_FRIENDLY:

                break;
            case SelectedAction.SELECTED_TARGET_ENEMY:

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
                if(onActionBtnClicked.theSkill != null)
                {
                    //Dispatch the skill used. The button itself could do it, but it's nice to keep it in One Statemachine!
                    OnCombatUISkillSelected skillChosenMessageData = new OnCombatUISkillSelected();
                    skillChosenMessageData.theSkill = onActionBtnClicked.theSkill;
                    Messenger.Dispatch(skillChosenMessageData);

                    //-- Then Display Targeting List
                }
                SetCurrentSelectedAction(SelectedAction.ATTACK);
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
        //so now we got here.. what to do?
        //should change state somewhere, since we now have chosen an Action
        //if we have pressed Attack the current Action selected is Attack
        //next Action is Select Target
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
