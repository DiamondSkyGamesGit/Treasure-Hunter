using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IDamageDealer, IDamageable, IKillable {

    public static Player Instance = null;
    public RPGSystem rpgSystem;

    public float Health =20f;
    public float Damage = 10.0f;
    public float MaxDamage = 25.0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        rpgSystem = GetComponent<RPGSystem>();

        DontDestroyOnLoad(gameObject);
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack()
    {
        
    }

    public void Attack(IDamageable target)
    {
        DealDamage(target, Random.Range(Damage, MaxDamage));
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void DealDamage(IDamageable target, float damage)
    {
        target.TakeDamage(damage);
    }

    /// <summary>
    /// Should handle Math around calculation of the players damage based on different variables and status effects active
    /// </summary>
    public void CalculatePlayerDamage()
    {

    }

    public void TakeDamageAnimation()
    {
        throw new System.Exception("NOT IMPLEMENTED TAKEDAMAGEANIMATION " + gameObject.name);
    }
}
