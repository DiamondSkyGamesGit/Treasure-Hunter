using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using System;

namespace HeroData { 
    /// <summary>
    /// Contains all hero data for the current hero
    /// </summary>
    [RequireComponent(typeof(HeroSkills))]

    [System.Serializable]
    public class Hero : MonoBehaviour, IPausable, IHasActionBar {

        //The Hero Name...
        public string heroName;
        
        //Testing for now, must change later into RPG system Initiative Roll with speed values etc
        //[Range(0, 1f)] public float actionBar = 0f;

        public float speed = 0.1f;

        //think i should do this as an interface with a property or method that heroes and enemies share in some way
        //then maybe controller could call all ICombatEntities to stop their actionBars..?
        public bool actionBarActive = false;
        public bool ActionPaused { get { return actionBarActive; } set { actionBarActive = value; } }

        private ActionBar actionBar;
        public ActionBar MyActionBar{ get { return actionBar; } set { actionBar = value; } }

        //--------Events--------------
        public delegate void OnActionBarFull(Hero theHero);
        public event OnActionBarFull onActionBarFull;
        
        //--------State machine-----------

        //the hero must report when it is ready to do something as defined by the state machine
        //dunno if i need this
        public enum ActionState
        {
            ACTION_BAR_CHARGING,
            ACTION_BAR_READY_WAIT_FOR_INPUT,
            ACTION_BAR_READY_QUEUED_ACTION
        }
        //default
        public ActionState actionState = ActionState.ACTION_BAR_CHARGING;

        //The hero's available Skills
        public HeroSkills heroSkills;

	    // Use this for initialization
	    void Start () {
            //-- Get Hero Skills --
            if (heroSkills == null) heroSkills = GetComponent<HeroSkills>();

            //-- Setup MyActionBar --
            //NOTE! initialize currentValue to a roll on initiative later, currently Random to spark dynamic combat situation
            MyActionBar = new ActionBar(0f, 1f, UnityEngine.Random.Range(0, 0.3f));
	    }

        void OnEnable()
        {
            //Add myself to GameController active heroes?
            CombatController.Instance.onBattleStateChanged += OnBattleStateChanged;
        }

        void OnDisable()
        {
            CombatController.Instance.onBattleStateChanged -= OnBattleStateChanged;
        }

        /// <summary>
        /// Handles what to do in this script based on which Battlestate we're in
        /// </summary>
        void OnBattleStateChanged(CombatController.BattleState battleState)
        {
            switch(battleState)
            {
                case (CombatController.BattleState.NORMAL_TIME_FLOW):
                    //do cool stuff
                    actionState = ActionState.ACTION_BAR_CHARGING;
                    PauseMe(true);
                    break;
                case (CombatController.BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):
                    PauseMe(false);
                    break;
            }
        }
	
	    void Update ()
        {
            //Increment the ActionBar
            if (actionBarActive)
            {
                float temp = MyActionBar.CurrentValue < MyActionBar.Max ? MyActionBar.CurrentValue : MyActionBar.Max;
                MyActionBar = new ActionBar(0f, 1f, temp + speed * Time.deltaTime);

                //if no player input have been given when action is ready, either
                //do default hero action
                //do last hero action again
                //do queued action
                //if ATB wait, then just pop up the UI menu when a players action is ready
            }
	    }

        public void PauseMe(bool amIPaused)
        {
            ActionPaused = amIPaused;
        }

        public bool AmIPaused()
        {
            return ActionPaused;
        }
    }
}
