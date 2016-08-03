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
    public ActionButton defaultEnemyActionButton;

    public CombatUIDefaultActions combatUIDefaultActions;
    public CombatUiTargetSelection combatUITargetSelection;

    //the sorted list on who has highest actionBarvalue
    public List<Hero> activeHeroesSorted = new List<Hero>();

    public bool showDebugLogs = false;


    //----------Mono Methods--------------
	void Start () {
	
        
	}

    void OnEnable()
    {
        CombatController.Instance.onBattleStateChanged += OnBattleStateChanged;
    }
	
    void OnDisable()
    {
        CombatController.Instance.onBattleStateChanged -= OnBattleStateChanged;
    }

	void Update () {
	
	}

    public void OnBattleStateChanged(CombatController.BattleState battleState)
    {
        switch (battleState)
        {
            case (CombatController.BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):

                //-- Get sorted hero list, pos[0] should be one with highest actionBarValue --
                activeHeroesSorted = GetSortedHeroListByActionBarValue(CombatController.Instance.activeHeroes);
                if (showDebugLogs) { 
                    foreach (var v in activeHeroesSorted)
                        Debug.Log(v.heroName + " " + v.MyActionBar.CurrentValue);
                }

                //-- Display scrollableActionList --
                EnablePlayerInputUI(activeHeroesSorted[0]);

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
        //either instantiate a prefab and set buttons on that prefab accordingly
        //or create prefabs for all buttons
        //keep them in a class
        //let that class deal with which buttons is instantiated
    }

    public void DisablePlayerInputUI()
    {

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
