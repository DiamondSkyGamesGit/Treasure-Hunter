using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;

namespace HeroData { 
    /// <summary>
    /// Contains all hero data for the current hero
    /// </summary>
    [RequireComponent(typeof(HeroSkills))]

    [System.Serializable]
    public class Hero : MonoBehaviour {

        //does this class need to be abstract?
        //dunno if i need derived implementations of each hero
        //create basic hero first so it's easy to derive from class if i need to

        //The Hero Name...
        public string heroName;
        
        //Testing for now, must change later into RPG system Initiative Roll with speed values etc
        [Range(0, 1f)] public float actionBar = 0f;
        public float speed = 0.1f;

        //think i should do this as an interface with a property or method that heroes and enemies share in some way
        //then maybe controller could call all ICombatEntities to stop their actionBars..?
        public bool actionBarActive = false;

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
            }
	    }
    }
}
