using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using HeroData;


/// <summary>
/// Handles the players control input during combat
/// namely the movement of selectors on the GUI and firing of events that says the player has in fact interacted with something
/// </summary>
public class PlayerCombatInput : MonoBehaviour {

    public static PlayerCombatInput Instance = null;

    //---------Events and Delegates----------

    //need to reassign this delegate to handle the buttonPress that the player pressed
    //might not need to notify from this class WHICH button was pressed right?
    //Then the button that is pressed tells UIController what to do forward
    //then this class just handles the actual Input, not what is done with the input

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


	}

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        //test pausing of Ipausable entities
        if(CombatController.Instance.currentBattleState == BattleState.NORMAL_TIME_FLOW) {
            if (Input.GetMouseButtonUp(0))
            {
                CombatController.Instance.OnPlayerInputPauseIPausables(true);
            }
        }

        if(CombatController.Instance.currentBattleState == BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT)
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                CombatController.Instance.OnPlayerInputPauseIPausables(false);
            }

            if(Input.GetKeyUp(KeyCode.X))
            {
                OnCombatUIChangeActiveHero temp = new OnCombatUIChangeActiveHero();
                temp.btnDirectionOnInput = CombatUIScrollDirection.RIGHT;
                Messenger.Dispatch(temp);
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                OnCombatUIChangeActiveHero temp = new OnCombatUIChangeActiveHero();
                temp.btnDirectionOnInput = CombatUIScrollDirection.LEFT;
                Messenger.Dispatch(temp);
            }
        }
    }

    void EnablePlayerInput()
    {

    }

    void DisablePlayerInput()
    {

    }


}
