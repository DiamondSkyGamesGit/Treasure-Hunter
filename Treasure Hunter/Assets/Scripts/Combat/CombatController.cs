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

    //??? how should i do this
    public List<Enemy> enemies = new List<Enemy>();

    #region ----My Messages----

    public OnCombatInitialized onCombatInitialized = new OnCombatInitialized();
    public OnCombatActiveHeroes onCombatActiveHeroes = new OnCombatActiveHeroes();
    public OnCombatActiveEnemies onCombatActiveEnemies = new OnCombatActiveEnemies();
    public OnBattleStateChanged onBattleStateChanged = new OnBattleStateChanged();

    #endregion

    #region ----BattleState State Machine------


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


    void OnEnable()
    {
        Messenger.AddListener<OnEnemyDie>(UpdateActiveEnemies);
        Messenger.AddListener<OnQueuedActionExecutingNow>(OnQueuedActionExecutingNow);
    }

    void OnDisable()
    {
        Messenger.RemoveListener<OnEnemyDie>(UpdateActiveEnemies);
        Messenger.RemoveListener<OnQueuedActionExecutingNow>(OnQueuedActionExecutingNow);
    }
    #endregion

    void OnQueuedActionExecutingNow(OnQueuedActionExecutingNow action)
    {
        PauseIPausables(true);
    }

    void OnQueuedActionExecuted(OnQueuedActionExecuted action)
    {
        PauseIPausables(false);
    }

    public void InitializeCombat(List<Enemy> activeEnemies)
    {
        theBattleCanvas.SetActive(true);

        //REMOVE LATER callback to subs whois active heroes
        //Can change later to Query Message
        activeHeroes = GameController.Instance.activeHeroes;
        onCombatActiveHeroes.activeHeroes = activeHeroes;
        Messenger.Dispatch(onCombatActiveHeroes);

        //tell all listeners that combat is now initiated
        Messenger.Dispatch(onCombatInitialized);

        //Callback to subscribers a list of the active enemies
        onCombatActiveEnemies.activeEnemies = activeEnemies;
        Messenger.Dispatch(onCombatActiveEnemies);

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
    public void SetCurrentBattleState(BattleState newBattleState)
    {
        //previous battleState, might wanna stop having a local var
        previousBattleState = currentBattleState;
        onBattleStateChanged.previousBattleState = currentBattleState;

        //set the now current battlestate
        onBattleStateChanged.currentBattleState = newBattleState;
        currentBattleState = newBattleState;

        Messenger.Dispatch(onBattleStateChanged);
        Debug.Log("Current Battlestate = " + currentBattleState);
    }

    void UpdateActiveEnemies(OnEnemyDie data)
    {
        //-- Call this method to ensure the item is actually NULL in the list, need a slight delay to ensure this
        StartCoroutine(CheckIfNullDelayed());
    }

    IEnumerator CheckIfNullDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        List<Enemy> temp = new List<Enemy>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                temp.Add(enemies[i]);
        }
        if(temp.Count <= 0)
        {
            //-- All enemies dead. Change state to player win
            SetCurrentBattleState(BattleState.PLAYER_WIN);

        }

        enemies.Clear();
        enemies = temp;
        OnCombatActiveEnemies dataclass = new OnCombatActiveEnemies();
        dataclass.activeEnemies = temp;
        Messenger.Dispatch(dataclass);
    }

    public void PauseIPausables(bool isPaused)
    {
        foreach (var v in activeHeroes) {
            v.PauseMe(isPaused);
        }
        if (isPaused)
            SetCurrentBattleState(BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT);
        else
            SetCurrentBattleState(BattleState.NORMAL_TIME_FLOW);
    }

    public void IAmDead(IKillable whoIsDead)
    {

    }
}
