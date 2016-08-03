using UnityEngine;
using System.Collections;

namespace SkillSystem {

    public interface ISkill {

        void SaveToSkillDatabase(Skill theSkill);
        void SaveToSkillDatabase(string theKey, Skill theSkill);
    }
}
