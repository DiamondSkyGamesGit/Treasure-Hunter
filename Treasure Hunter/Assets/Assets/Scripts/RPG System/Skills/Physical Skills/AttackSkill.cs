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

        }

        public AttackSkill(string skillName, float _baseDmg, float _chargeTime) :
            base(skillName, _baseDmg, _chargeTime)
        {

        }
    }
}
