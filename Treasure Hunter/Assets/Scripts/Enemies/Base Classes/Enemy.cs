using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Enemy : MonoBehaviour, IKillable, IDamageable, IDamageDealer, ITargetable
{

    private Renderer myRenderer;

    [SerializeField]
    private string enemyName;
    public string EnemyName { get { return enemyName; }protected set { enemyName = value; } }
    [SerializeField]
    private float health;
    public float Health { get { return health; } protected set { health = value; } }
    [SerializeField]
    private float baseDamage;
    public float BaseDamage { get { return baseDamage; } protected set { baseDamage = value; } }
    [SerializeField]
    private float maxDamage;
    public float MaxDamage { get { return maxDamage; } protected set { maxDamage = value; } }

    private bool isTargetable = true;
    public bool IsTargetable { get { return isTargetable; } set { isTargetable = value; } }

    private TargetType _targetType;
    public TargetType targetType { get { return _targetType; } set { _targetType = value; } }

    public Color defaultColor;
    public Color takeDamageTargetColor;

    public float takeDamageAnimationSpeed = .2f;


    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        defaultColor = myRenderer.material.color;//might have to be done in child classes, unsure, depends on how i use materials on different enemies
    }

    void Start()
    {

        _targetType = TargetType.ENEMY;
    }




    //-----------------Deal Damage Methods-----------------

    public virtual void Attack(IDamageable target)
    {

        DealDamage(target, Random.Range(BaseDamage, MaxDamage));
    }



    public void DealDamage(IDamageable target, float damage)
    {
        target.TakeDamage(damage);
    }

    //--------------------Take Damage Methods----------------

    public void TakeDamage(float damage)
    {
        Debug.Log("Ouch! " + this.gameObject.name + " Got hit for " + damage + " damage! Current health = " + Health);
        Health -= damage;
        TakeDamageAnimation();
        Debug.Log("Health after hit = " + Health);
        if (Health <= 0)
            Die();
    }

    public void TakeDamageAnimation()
    {
        StartCoroutine(PlayDamageAnimation(takeDamageAnimationSpeed));
    }

    protected virtual IEnumerator PlayDamageAnimation(float _lerpTime)
    {
        float lerpTime = _lerpTime;
        float currentLerpTime = 0f;

        while(myRenderer.material.color != takeDamageTargetColor)
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
                currentLerpTime = lerpTime;

            float perc = currentLerpTime / lerpTime;

            myRenderer.material.color = Color.Lerp(defaultColor, takeDamageTargetColor, perc);
            yield return null;
        }
        myRenderer.material.color = defaultColor;
       
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
