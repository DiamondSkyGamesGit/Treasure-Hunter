using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;

namespace HeroData { 

    /// <summary>
    /// Frontend for using the SkillSystem.
    /// All interactions with the skillsystem is done in this class.
    /// Add component to a PlayableHero gameobject
    /// </summary>
    public class HeroSkills : MonoBehaviour {

        //certain classes can be coupled since they are so relevant to each other
        //but would be sad if a character using skills is not a Hero?
        //ideal situation is that an enemy or boss can also use skills
        //but an enemy has their own implementation of attack where they send a skill they currently have

        //So when player presses Attack, the ActionButton must either have a reference to this class and get the AttackSkill
        //or
        //the attackButton launches an event
        public List<Skill> mySkills = new List<Skill>();
        public Dictionary<Skill, string> mySkillsCollection = new Dictionary<Skill, string>();

	    // Use this for initialization
	    void Start () {
            mySkillsCollection.Add(new AttackSkill("Attack", 20f, 0.5f), "attackSkill");
            
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }
    }
}
