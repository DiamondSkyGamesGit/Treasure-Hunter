using UnityEngine;
using System.Collections;

/// <summary>
/// Contains all RPG statistics for the object attached
/// </summary>
public class RPGSystem : MonoBehaviour {

    public DamageModifiers damageModifiers;

    [SerializeField]private int level;
    public int Level { get { return level; } set { level = value; } }

    [SerializeField]private float experience;
    public float Experience { get { return experience; } set { experience = value; } }

    [SerializeField]private float experienceToNextLevel;
    public float ExperienceToNextLevel { get { return experienceToNextLevel; } set { experienceToNextLevel = value; } }

    public void GainExperience(float amount)
    {

    }

    public void LevelUp()
    {

    }

	// Use this for initialization
	void Start () {
        damageModifiers = new DamageModifiers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [System.Serializable]
    public class DamageModifiers
    {
        public float fireDamage;

        public DamageModifiers()
        {

        }
    }
}
