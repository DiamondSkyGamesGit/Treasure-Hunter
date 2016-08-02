using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;

namespace HeroData { 
    /// <summary>
    /// Contains all hero data for the current hero
    /// </summary>
    [RequireComponent(typeof(HeroSkills))]
    public class Hero : MonoBehaviour {

        //does this class need to be abstract?
        //dunno if i need derived implementations of each hero
        //create basic hero first so it's easy to derive from class if i need to
        public HeroSkills heroSkills;

	    // Use this for initialization
	    void Start () {

            if (heroSkills == null) heroSkills = GetComponent<HeroSkills>();
	
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }
    }
}
