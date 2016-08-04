using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HeroData;

/// <summary>
/// This class holds the current combat state
/// Handles pausing of other scripts when the player is in PlayerInputMode
/// Has references to all relevant combat entities as Enemies, Heroes
/// Fires events when CombatState changes
/// </summary>
public class CombatController : MonoBehaviour {

    public static CombatController Instance = null;

    public GameObject theBattleCanvas;
    /// <summary>
    ///the time stayed in BattleState.CombatIntroduction before changing state
    ///Allows for intro animations, camera movement
    /// </summary>
    public float combatIntroTime = 3f;

    //The active Heroes
    public List<Hero> activeHeroes = new List<Hero>();

    // public IDamageDealer[] combatants;
    public List<IDamageDealer> combatants = new List<IDamageDealer>();
    public List<Enemy> enemies = new List<Enemy>();
    // i need to know whose turn it is


    #region ----Events and Delegates----

    public delegate void OnCombatActiveHeroes(List<Hero> activeHeroes);
    public event OnCombatActiveHeroes onCombatActiveHeroes;

    //if hero party changes during combat this must be fired to subs. Ensure to desubscribe listeners to the previous active heroes list
    public delegate void OnCombatActiveHeroesChanged(List<Hero> previousActiveHeroes, List<Hero> activeHeroes);
    public event OnCombatActiveHeroesChanged onCombatActiveHeroesChanged;


    //fired whenever amount of enemies change as well
    public delegate void OnCombatActiveEnemies(List<Enemy> enemyList);
    public event OnCombatActiveEnemies onCombatActiveEnemies;

    public delegate void OnCombatInitiated();
    public event OnCombatInitiated onCombatInitiated;

    public delegate void OnBattleStateChanged(BattleState combatState);
    public event OnBattleStateChanged onBattleStateChanged;
    #endregion


    #region ----BattleState State Machine------
    public enum BattleState
    {
        NOT_COMBAT,
        COMBAT_INTRODUCTION,
        NORMAL_TIME_FLOW,
        PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT,
        PLAYER_WIN,
        PLAYER_LOOSE
    }

    public BattleState previousBattleState;
    public BattleState currentBattleState;

    #endregion

    #region --Mono Behaviours--
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	void Start () {

	}
	
	void Update () {
	
	}

    void OnEnable()
    {
        
    }

    void OnDisable()
    {

    }
    #endregion

    public void InitializeCombat(List<Enemy> activeEnemies)
    {
        theBattleCanvas.SetActive(true);

        //REMOVE LATER callback to subs whois active heroes
        activeHeroes = GameController.Instance.activeHeroes;
       // onCombatActiveHeroes(activeHeroes); might get null ref because no subs yet

        //tell all listeners that combat is now initiated
        if (onCombatInitiated != null)
            onCombatInitiated();

        //Callback to subscribers a list of the active enemies
        if(onCombatActiveEnemies != null)
            onCombatActiveEnemies(activeEnemies);

        //change BattleState
        SetCurrentBattleState(BattleState.COMBAT_INTRODUCTION);
        StartCoroutine(CombatIntroduction(combatIntroTime));
    }

    /// <summary>
    /// Introduces combat
    /// </summary>
    /// <param name="_combatIntroTime"></param>
    /// <returns></returns>
    IEnumerator CombatIntroduction(float _combatIntroTime)
    {
        //start music?
        //do cool stuff?
        
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        while(timer.Elapsed.TotalSeconds <= _combatIntroTime)
        {
            yield return null;
        }
        //shouldn't be necessary to reset timer, should be killed when leaving method

        //Change BattleState
        SetCurrentBattleState(BattleState.NORMAL_TIME_FLOW);
    }

    /// <summary>
    /// Sets battlestate
    /// sets previous battlestate for convenience
    /// Fires event onBattleStateChanged
    /// </summary>
    /// <param name="curBattleState"></param>
    public void SetCurrentBattleState(BattleState curBattleState)
    {
        previousBattleState = currentBattleState;
        currentBattleState = curBattleState;
        onBattleStateChanged(currentBattleState);
        Debug.Log("Current Battlestate = " + currentBattleState);
    }

    public void OnPlayerInputPauseIPausables(bool isPaused)
    {
        foreach (var v in activeHeroes)
            v.PauseMe(isPaused);
        if (isPaused)
            SetCurrentBattleState(BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT);
        else
            SetCurrentBattleState(BattleState.NORMAL_TIME_FLOW);
    }

    public void IAmDead(IKillable whoIsDead)
    {

    }
}
