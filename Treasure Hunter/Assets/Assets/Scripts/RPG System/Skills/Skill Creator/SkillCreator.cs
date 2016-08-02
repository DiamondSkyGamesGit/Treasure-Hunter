﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SkillSystem { 
    /// <summary>
    /// this is a workaround for now on creating skills.
    /// skills as default implementations are created here, then users of SkillSystem can get the skill from SkillDatabase
    /// keep as monobehavior to let me create skills from SkillDatabase in inspector
    /// </summary>
    public class SkillCreator : MonoBehaviour {

        public AttackSkill attackSkill;
        public bool DebugGetAllValuesFromDatabase = false;

	    // Use this for initialization
	    void Start () {
            attackSkill = new AttackSkill("Attack", 20f, 0.5f);
            attackSkill.SaveToSkillDatabase(attackSkill);
            Debug.Log(SkillDatabase.Instance);
	    }
	
	    // Update is called once per frame
	    void Update () {
	    
            if(DebugGetAllValuesFromDatabase)
            {
                foreach(var v in SkillDatabase.Instance.skillCollection)
                {
                    Debug.Log(v);
                }
                DebugGetAllValuesFromDatabase = false;
            }
	    }
    }
}
