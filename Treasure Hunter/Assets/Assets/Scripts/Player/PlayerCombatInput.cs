using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// Handles CombatCanvas UI
/// Button clicks calls methods on this script through static Unity Events
/// </summary>
public class PlayerCombatInput : MonoBehaviour {

    public static PlayerCombatInput Instance = null;

    //---------Events and Delegates----------
    //should create delegates that turn manager listens to

    public delegate void OnClickAttack();
    public event OnClickAttack onClickAttack;

    //Should listen to an event from turnManager that enables UI control when it's the player's turn

    //---------UI Fields------------
    public Canvas combatCanvas;

    //--------Enemies------
    public List<Enemy> activeEnemies = new List<Enemy>();

    //-----------Layermasks------------
    public LayerMask enemyLayer;

    //---------Targeting------------
    public TargetSelector targetSelector;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        if (combatCanvas.gameObject.activeSelf)
            DisablePlayerInput();

        CombatController.Instance.onIsPlayersTurn += EnablePlayerInput;
        CombatController.Instance.onCombatActiveEnemies += GetEnemyListFromCallback;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        //subscribe to turnmanagers onIsPlayersTurn, delegate method allowing player input from here
        //TurnManager.Instance.onIsPlayersTurn += EnablePlayerInput;
        CombatController.Instance.onCombatActiveEnemies += GetEnemyListFromCallback;
    }

    void OnDisable()
    {
        CombatController.Instance.onIsPlayersTurn -= EnablePlayerInput;
        CombatController.Instance.onCombatActiveEnemies -= GetEnemyListFromCallback;
        if (combatCanvas.gameObject.activeSelf)
            DisablePlayerInput();
    }

    void EnablePlayerInput()
    {
        combatCanvas.gameObject.SetActive(true);
    }

    void DisablePlayerInput()
    {
        combatCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Listens to turnManager, gets list of active enemies from event callback
    /// </summary>
    /// <param name="enemies"></param>
    void GetEnemyListFromCallback(List<Enemy> enemies)
    {
        activeEnemies = enemies;
    }

    void SelectTarget()
    {
    }

    IEnumerator SelectTargetWaitForInput()
    {
        bool inputGiven = false;
        while(!inputGiven)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 20000f, enemyLayer, QueryTriggerInteraction.Ignore))
                {
                    //change this later
                    Debug.Log(hit.collider.tag);
                    if (hit.collider.tag == "Enemy") {
                        targetSelector.SelectTarget(hit.collider.gameObject.transform);
                        Player.Instance.DealDamage((IDamageable)hit.collider.GetComponent<Enemy>(), Player.Instance.Damage);
                    }
                    inputGiven = true;
                }

            }
            yield return null;
        }
    }

    /// <summary>
    /// Called from UnityEvent static button
    /// </summary>
    public void Attack()
    {
        if(onClickAttack != null)
            onClickAttack();//Fire event
        StartCoroutine(SelectTargetWaitForInput());
    }
}
