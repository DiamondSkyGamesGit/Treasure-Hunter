using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class holds the current combat state
/// Handles pausing of other scripts when the player is in PlayerInputMode
/// Has references to all relevant combat entities as Enemies, Heroes
/// Fires events when CombatState changes
/// </summary>
public class CombatController : MonoBehaviour {

    public static CombatController Instance = null;


    /// <summary>
    ///the time stayed in BattleState.CombatIntroduction before changing state
    ///Allows for intro animations, camera movement
    /// </summary>
    public float combatIntroTime = 3f;


    // public IDamageDealer[] combatants;
    public List<IDamageDealer> combatants = new List<IDamageDealer>();
    public List<Enemy> enemies = new List<Enemy>();
    // i need to know whose turn it is


    //--------Events and Delegates-------------


    //fired whenever amount of enemies change as well
    public delegate void OnCombatActiveEnemies(List<Enemy> enemyList);
    public event OnCombatActiveEnemies onCombatActiveEnemies;

    public delegate void OnCombatInitiated();
    public event OnCombatInitiated onCombatInitiated;

    public delegate void OnCombatStateChanged(BattleState combatState);
    public event OnCombatStateChanged onCombatStateChanged;

    //------------State machine
    public enum BattleState
    {
        NOT_COMBAT,
        COMBAT_INTRODUCTION,
        NORMAL_TIME_FLOW,
        PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT,
        PLAYER_WIN,
        PLAYER_LOOSE
    }

    public BattleState battleState;

    #region --Mono Behaviours--
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);
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
        //tell all listeners that combat is now initiated
        if (onCombatInitiated != null)
            onCombatInitiated();

        //Callback to subscribers a list of the active enemies
        if(onCombatActiveEnemies != null)
            onCombatActiveEnemies(activeEnemies);

        //change CombatState
        battleState = BattleState.COMBAT_INTRODUCTION;
    }

    /// <summary>
    /// The intro time before combat actually starts
    /// </summary>
    /// <param name="_combatIntroTime"></param>
    /// <returns></returns>
    IEnumerator CombatIntroduction(float _combatIntroTime)
    {
        //start music?
        //do cool stuff?
        //better to write longer stuff here than import Sys.Diag and have to write alot to use Debug.Log....
        
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        while(timer.Elapsed.TotalSeconds <= _combatIntroTime)
        {
            yield return null;
        }
        
    }

    IEnumerator WaitForPlayerInput()
    {
        bool playerHasGivenInput = false;

        while (!playerHasGivenInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Player.Instance.Attack((IDamageable)combatants[0]);
                playerHasGivenInput = true;
            }
            yield return null;
        }
        //Delete this, just for testing.
        StartCoroutine(WaitForPlayerInput());
    }
}
