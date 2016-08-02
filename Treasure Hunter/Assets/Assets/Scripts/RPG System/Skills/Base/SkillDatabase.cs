using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SkillSystem {
    /// <summary>
    /// Users must add a skill to database when creating a new skill
    /// </summary>
    [System.Serializable]
    public class SkillDatabase:MonoBehaviour {

        public static SkillDatabase Instance = null;

        /// <summary>
        /// Do not add skills directly
        /// </summary>
        public Dictionary<string, Skill> skillCollection = new Dictionary<string, Skill>();
        /// <summary>
        /// users can look up a key, instead of static typing when getting a Skill to save programming errors
        /// </summary>
        public List<string> skillCollectionKeys = new List<string>();

        void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

	    // Use this for initialization
	    void Start () {
	
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        public void AddSkillToCollection(string theKey, Skill theSkill)
        {
            skillCollection.Add(theKey, theSkill);
            skillCollectionKeys.Add(theKey);
        }

        public void RemoveSkillFromCollection(string theKey)
        {
            if (skillCollection.ContainsKey(theKey)) skillCollection.Remove(theKey);
        }
    }
}
