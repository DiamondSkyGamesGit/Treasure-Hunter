using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all weapon types
/// </summary>
///
[System.Serializable]
public class Weapon : MonoBehaviour {

    [SerializeField]private float baseDamage;
    public float BaseDamage { get { return baseDamage; } set { baseDamage = value; } }

    [ReadOnly]public float minDamage;
    public float MinDamage { get { return minDamage; } set { minDamage = value; } }
    [ReadOnly]public float maxDamage;
    public float MaxDamage { get { return maxDamage; } set { maxDamage = value; } }

   [ReadOnly] public float averageDamage;
    public float AverageDamage { get { return MinDamage + MaxDamage / 2; } set { averageDamage = value; } }
    public float WeaponDPS { get { return AverageDamage / AttackSpeed; } }

    [SerializeField]
    private float attackSpeed;
    public float AttackSpeed { get { return attackSpeed; }set { attackSpeed = value; } }

    public string weaponName = "default weapon name";

    public void CreateWeapon(string _weaponName, float baseDmg,float minDmg, float maxDmg, float attackSpd)
    {
        weaponName = _weaponName;
        BaseDamage = baseDmg;
        MinDamage = minDmg;
        MaxDamage = maxDmg;
        AttackSpeed = attackSpd;
    }

	// Use this for initialization
	void Start () {
        CreateWeapon("The Beast", 50f, 25f, 75f, 2.5f);
        averageDamage = AverageDamage;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
