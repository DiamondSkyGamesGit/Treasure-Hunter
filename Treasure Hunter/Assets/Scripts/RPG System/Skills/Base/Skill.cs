using UnityEngine;
using System.Collections;
using System;

namespace SkillSystem { 
    /// <summary>
    /// The base class for all skills. 
    /// </summary>
    [System.Serializable]
    public class Skill : ScriptableObject
    {
        public string skillName;
        public float baseDamage;

    }
}
