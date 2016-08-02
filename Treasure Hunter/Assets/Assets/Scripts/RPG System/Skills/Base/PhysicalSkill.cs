using UnityEngine;
using System.Collections;
using System;
namespace SkillSystem {

    [System.Serializable]
    public class PhysicalSkill : Skill, IPhysicalSkill
    {

       [SerializeField][ReadOnly] private float baseDamage;


        public float BaseDamage
        {
            get
            {
                return baseDamage;
            }
        }

        //---------Constructors---------
        public PhysicalSkill():base()
        {
            baseDamage = 10f;//default is not in Skill because all skills do not cause damage i presume
        }

        public PhysicalSkill(string _skillName, float _baseDamage, float _chargeTime)
            :base(_skillName, _chargeTime)
        {
            baseDamage = _baseDamage;
        }

        public PhysicalSkill(float _baseDmg)
        {
            this.baseDamage = _baseDmg;
        }


        //-------Methods--------
        public override void UseSkill()
        {
            throw new NotImplementedException();
        }

    }
}
