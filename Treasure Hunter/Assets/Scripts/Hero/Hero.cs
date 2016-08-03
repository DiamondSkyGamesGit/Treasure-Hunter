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
    public class Hero : MonoBehaviour, IPausable {

        //The Hero Name...
        public string heroName;
        
        //Testing for now, must change later into RPG system Initiative Roll with speed values etc
        [Range(0, 1f)] public float actionBar = 0f;

        //prolly change ActionBar to a struct
        [System.Serializable]
        public struct ActionBar { public float min; public float max; public ActionBar(float _min, float _max) { min = _min;max = _max; } }
        public ActionBar theActionBar;
        public float speed = 0.1f;

        //think i should do this as an interface with a property or method that heroes and enemies share in some way
        //then maybe controller could call all ICombatEntities to stop their actionBars..?
        public bool actionBarActive = false;
        public bool ActionPaused { get { return actionBarActive; } set { actionBarActive = value; } }

        //--------Events--------------
        public delegate void OnActionBarFull(Hero theHero);
        public event OnActionBarFull onActionBarFull;
        
        //--------State machine-----------

        //the hero must report when it is ready to do something as defined by the state machine
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

            if (heroSkills == null) heroSkills = GetComponent<HeroSkills>();
	
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

        void OnBattleStateChanged(CombatController.BattleState battleState)
        {
            switch(battleState)
            {
                case (CombatController.BattleState.NORMAL_TIME_FLOW):
                    //do cool stuff
                    actionState = ActionState.ACTION_BAR_CHARGING;
                    actionBarActive = true;
                    break;
                case (CombatController.BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):
                    actionBarActive = false;
                    break;
            }
        }
	
	    void Update ()
        {
            if (actionBarActive)
            {
                actionBar += speed * Time.deltaTime;
                if (actionBar > 1f)
                    actionBar = 1f;

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
