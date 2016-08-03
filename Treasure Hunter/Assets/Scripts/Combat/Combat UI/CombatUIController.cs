using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
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
    public ActionButton defaultEnemyActionButton;

    public CombatUIOffensiveActions combatUIOffensiveActions;
    public CombatUiTargetSelection combatUITargetSelection;


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
                //-- Display scrollableActionList
                List<Hero> temp = GetSortedHeroListByActionBarValue(CombatController.Instance.activeHeroes);
                foreach (var v in temp)
                    Debug.Log(v.heroName + " " + v.actionBar);
                break;
        }
    }

    //Unsure if this works, test later with more Heroes
    private List<Hero> GetSortedHeroListByActionBarValue(List<Hero> heroes)
    {
        heroes.Sort(delegate (Hero a, Hero b) { return (a.actionBar).CompareTo(b.actionBar); });
        List<Hero> temp = new List<Hero>();
        temp = heroes;
        return temp;
        
    }

    public void EnablePlayerInputUI()
    {

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
