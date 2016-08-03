using UnityEngine;
using System.Collections;

namespace SkillSystem { 

    /// <summary>
    /// Defines a physical attack skill
    /// </summary>
    [System.Serializable]
    public class AttackSkill : PhysicalSkill {


        public AttackSkill():base()
        {
            //uh oh this skill can be created several times for each hero...........................
            //i must have an object that knows each skill available in the game, probably should get it from Save....
            //or create for now a SkillCreatorClass that implements a default construction for each possible skill,
            //then HeroSkills just gets the Skills it needs from SkillDatabase as the default implementation
        }

        public AttackSkill(string skillName, float _baseDmg, float _chargeTime) :
            base(skillName, _baseDmg, _chargeTime)
        {

        }
    }
}
