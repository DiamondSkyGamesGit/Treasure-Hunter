using UnityEngine;
using System.Collections;
using System;

namespace SkillSystem { 
    /// <summary>
    /// The base class for all skills. 
    /// </summary>
    [System.Serializable]
    public abstract class Skill : ISkill {

        //does it need to be a monobehavior?
        //if is monobehavior, can create alot of skill prefabs that are empty game objects that are instantiated
        //if !monobehavior, then must create constructor based system

        //let's try non monobehavior system first

        /// <summary>
        /// A skill contains:
        /// Derived classes that represent their direction
        /// is a passable collection of data so that when a player uses a skill, the skill effects is "used" on the target
        /// is "equippable" on a hero character
        /// </summary>
        /// 
        [SerializeField][ReadOnly]
        string skillName;
        public string SkillName { get { return skillName; } }

        //A cast time of 0 means: instant cast when attack is done
        //A cast time > 0 means: the Action is Cast when the castTimer >= castTime
        [SerializeField]
        [ReadOnly]
        float castTime;
        public float CastTime { get { return castTime; } }

        public Skill()
        {
            skillName = "default skill name";
            castTime = 0f;
        }

        public Skill(string _skillName, float _chargeTime)
        {
            skillName = _skillName;
            castTime = _chargeTime;
        }

        public abstract void UseSkill();

        /// <summary>
        /// Skill is saved with key as Skill.SkillName
        /// derived classes should supply themselves
        /// </summary>
        public virtual void SaveToSkillDatabase(Skill theSkill)
        {
            if(SkillDatabase.Instance != null)
            {
                SkillDatabase.Instance.AddSkillToCollection(theSkill.SkillName, theSkill);
            }
        }

        /// <summary>
        /// Skill is saved with string<param name="key"> key</param>
        /// derived classes should supply themselves
        /// </summary>
        public virtual void SaveToSkillDatabase(string key, Skill theSkill)
        {
            if(SkillDatabase.Instance != null)
            {
                SkillDatabase.Instance.AddSkillToCollection(key, theSkill);
            }
        }
    }
}
