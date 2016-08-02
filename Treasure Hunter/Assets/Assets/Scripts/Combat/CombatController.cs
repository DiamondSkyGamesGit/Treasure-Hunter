using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class holds the current combat state
/// Handles pausing of other scripts when the player is in PlayerInputMode
/// Has references to all relevant combat entities as Enemies, Heroes
/// Fires events when CombatState changes
/// </summary>
public class CombatController : MonoBehaviour {

    public static CombatController Instance = null;


    // public IDamageDealer[] combatants;
    public List<IDamageDealer> combatants = new List<IDamageDealer>();
    public List<Enemy> enemies = new List<Enemy>();
    // i need to know whose turn it is


    //--------Events and Delegates-------------
    public delegate void OnIsPlayersTurn();
    public event OnIsPlayersTurn onIsPlayersTurn;

    //fired whenever amount of enemies change as well
    public delegate void OnCombatActiveEnemies(List<Enemy> enemyList);
    public event OnCombatActiveEnemies onCombatActiveEnemies;

    public delegate void OnCombatInitiated();
    public event OnCombatInitiated onCombatInitiated;

    public delegate CombatState OnCombatStateChanged();

    //------------State machine
    public enum CombatState
    {
        NOT_COMBAT,
        NORMAL_TIME_FLOW,
        PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT,
        PLAYER_WIN,
        PLAYER_LOOSE
    }

    public CombatState combatState;

    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        //PlayerCombatInput.Instance.onClickAttack
    }

    void OnDisable()
    {

    }

    public void InitializeCombat(List<Enemy> activeEnemies)
    {
        if (onCombatInitiated != null)
            onCombatInitiated();

        //Callback to subscribers a list of the active enemies
        if(onCombatActiveEnemies != null)
            onCombatActiveEnemies(activeEnemies);


        StartTurn();
    }

    List<Enemy> onActiveEnemiesHandler(List<Enemy> enemies)
    {
        return enemies;
    }


    public void StartTurn()
    {
        if (onIsPlayersTurn != null)
            onIsPlayersTurn();
        //StartCoroutine(WaitForPlayerInput());

    }

    IEnumerator WaitForPlayerInput()
    {
        bool playerHasGivenInput = false;

        Debug.Log(onIsPlayersTurn);
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

    void NextTurn()
    {

    }

    void EndTurn()
    {

    }
}
