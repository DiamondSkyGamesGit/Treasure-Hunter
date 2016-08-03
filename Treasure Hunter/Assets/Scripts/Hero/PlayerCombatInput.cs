using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using HeroData;


/// <summary>
/// Handles the players control input during combat
/// </summary>
public class PlayerCombatInput : MonoBehaviour {

    public static PlayerCombatInput Instance = null;

    //---------Events and Delegates----------

    //need to reassign this delegate to handle the buttonPress that the player pressed
    //might not need to notify from this class WHICH button was pressed right?
    //Then the button that is pressed tells UIController what to do forward
    //then this class just handles the actual Input, not what is done with the input
    public delegate void OnClickAttack();
    public event OnClickAttack onClickAttack;

    //Should listen to an event from turnManager that enables UI control when it's the player's turn

    //---------UI Fields------------
    public Canvas combatCanvas;

    //--------Enemies------
    public List<Enemy> activeEnemies = new List<Enemy>();

    //-------Hero Data-------------
    //The selected hero is the hero which the player uses the CombatUI for (like FFXII)
    public Hero currentSelectedHero;

    //---------Targeting------------
    //should be moved out of class
    public TargetSelector targetSelector;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {

        CombatController.Instance.onCombatActiveEnemies += GetEnemyListFromCallback;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        //subscribe to turnmanagers onIsPlayersTurn, delegate method allowing player input from here
        //TurnManager.Instance.onIsPlayersTurn += EnablePlayerInput;
        CombatController.Instance.onCombatActiveEnemies += GetEnemyListFromCallback;
    }

    void OnDisable()
    {
        CombatController.Instance.onCombatActiveEnemies -= GetEnemyListFromCallback;
    }

    void EnablePlayerInput()
    {
        combatCanvas.gameObject.SetActive(true);
    }

    void DisablePlayerInput()
    {
        combatCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Listens to turnManager, gets list of active enemies from event callback
    /// </summary>
    /// <param name="enemies"></param>
    void GetEnemyListFromCallback(List<Enemy> enemies)
    {
        activeEnemies = enemies;
    }

    void SelectTarget()
    {
    }


    /// <summary>
    /// Called from UnityEvent static button
    /// </summary>
    public void Attack()
    {
        if(onClickAttack != null)
            onClickAttack();//Fire event
        
    }
}
