using UnityEngine;

public class EnemyTest : Enemy {

   public float myHealth { get { return myHealth; } }
	// Use this for initialization
	protected override void Start () {

        //KUL KODE
        /*
        foreach(var i in typeof(Enemy).GetInterfaces())
        {
            Debug.Log(i);
        }
        */
        base.Start();
	}

    public override void Attack(IDamageable target)
    {
        base.Attack(target);
    }

    // Update is called once per frame
    void Update () {
	
	}
    
    public override void Die()
    {
        OnEnemyDie temp = new OnEnemyDie();
        Messenger.Dispatch(temp);

        base.Die();
    }
    
}
