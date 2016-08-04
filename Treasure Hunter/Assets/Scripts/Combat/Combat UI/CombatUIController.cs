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

    //the sorted list on who has highest actionBarvalue
    public List<Hero> activeHeroesSorted = new List<Hero>();
    //if i am to use activeHero it is very important to update active hero correctly after button listeners have been assigned to the hero's buttons
    //might not be a good system
    public Hero activeHero;
    public List<ActionButton> currentActionButtons = new List<ActionButton>();

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
        // will give null ref on start unless battlecanvas is not enabled

    }
    void OnDisable()
    {
        CombatController.Instance.onBattleStateChanged -= OnBattleStateChanged;

    }

	void Update () {
	
	}
    #endregion


    #region --//-- Add & Remove Listeners Methods --\\--

    
    List<ActionButton> GetActiveHeroDefaultActionButtons(Hero theHero)
    {
        return theHero.heroSkills.combatUIDefaultActions.actionButtons;
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

    public void OnBattleStateChanged(CombatController.BattleState battleState)
    {
        switch (battleState)
        {
            case (CombatController.BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):

                //-- Get sorted hero list, pos[0] should be one with highest actionBarValue --
                activeHeroesSorted = GetSortedHeroListByActionBarValue(CombatController.Instance.activeHeroes);
                activeHero = activeHeroesSorted[0];

                if (showDebugLogs) { 
                    foreach (var v in activeHeroesSorted)
                        Debug.Log(v.heroName + " " + v.MyActionBar.CurrentValue);
                }


                //-- Set currentActionButtons
                currentActionButtons = GetActiveHeroDefaultActionButtons(activeHero);

                //-- Display scrollableActionList --
                EnablePlayerInputUI(activeHero);

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

    #region --//-- Input Handling Methods --\\--

    /// <summary>
    /// start input at Hero.First or Partyleader for method with no params
    /// </summary>
    public void EnablePlayerInputUI()
    {

    }

    /// <summary>
    /// starts input UI at theHero
    /// </summary>
    /// <param name="theHero"></param>
    public void EnablePlayerInputUI(Hero theHero)
    {
        //-- Enable ScrollableActionList --
        scrollableActionList.gameObject.SetActive(true);

        //-- Set textObject to represent HeroName
        activeHeroNameTextObj.text = theHero.heroName;

        //-- Get available actions from Hero and display accordingly in ScrollableActionList

        
        scrollableActionList.InstantiateAndDisplayItems(currentActionButtons);
        //add listeners to the buttons
        AddButtonListeners(scrollableActionList.actionButtons);
    }
    //either instantiate a prefab and set buttons on that prefab accordingly
    //or create prefabs for all buttons
    //keep them in a class
    //let that class deal with which buttons is instantiated


    public void DisablePlayerInputUI()
    {
        RemoveButtonListeners(GetActiveHeroDefaultActionButtons(activeHero));
    }

    #endregion

    public void OnActionButtonClicked(ActionButton theButton)
    {
        Debug.Log("Got tha message!");
    }

    /// <summary>
    /// Are called when CombatState == 
    /// </summary>
    void DisplayOffensiveActions()
    {

    }

    void DisplayTargetSelection()
    {
        //scrollableActionList.DisplayActions<ActionButton>(combatUITargetSelection.activeEnemies);
    }


}
