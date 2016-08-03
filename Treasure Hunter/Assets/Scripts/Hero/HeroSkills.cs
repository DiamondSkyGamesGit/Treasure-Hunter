using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SkillSystem;

namespace HeroData { 

    /// <summary>
    /// Frontend for using the SkillSystem.
    /// All interactions with the skillsystem is done in this class.
    /// Add component to a PlayableHero gameobject
    /// </summary>
    [System.Serializable]
    public class HeroSkills : MonoBehaviour {

        //certain classes can be coupled since they are so relevant to each other
        //but would be sad if a character using skills is not a Hero?
        //ideal situation is that an enemy or boss can also use skills
        //but an enemy has their own implementation of attack where they send a skill they currently have

        //So when player presses Attack, the ActionButton must either have a reference to this class and get the AttackSkill
        //or
        //the attackButton launches an event
        public List<Skill> mySkills = new List<Skill>();
        public Dictionary<string, Skill> mySkillsCollection = new Dictionary<string, Skill>();
        public CombatUIDefaultActions combatUIDefaultActions;

	    // Use this for initialization
	    void Start () {
            //i think that certain skills are so default i can statically type them here,
            //but later when using special skills must do a better way of registering available skills to hero
            mySkillsCollection.Add(StringsLibrary.AttackSkill, SkillSystem.SkillDatabase.Instance.GetSkillFromCollection(StringsLibrary.AttackSkill));
            
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public Skill GetSkill(string key)
        {
            foreach(var v in mySkillsCollection)
            {
                if (v.Key == key)
                    return v.Value;
            }
            return null;
        }

        /*Does not work now....
        public Skill GetSkillByType<T>() where T : Skill
        {
            foreach(KeyValuePair<string, Skill> v in mySkillsCollection)
            {
                if (v.GetType() is T)
                    return v.Value;
            }
            return null;
        }
        */
    }
}
