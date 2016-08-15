using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A combat encounter is instantiated for each combat
/// An encounter contains the enemies that the player battles
/// A combat encounter can be placed on each encounter area, for example around an enemy or something
/// </summary>
public class CombatEncounter : MonoBehaviour {

    public List<Enemy> enemies = new List<Enemy>();
    public Vector3 targetCameraPosition;
    public bool debugShowGizmos = false;
    private bool combatStarted = false;

	// Use this for initialization
	void Start () {
	    
        //Say to TurnManager which enemies are active and the player
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !combatStarted)
            StartCombatEncounter();
    }

    public void StartCombatEncounter()
    {
        Debug.Log("Combat initiated");
        foreach (var e in enemies) {
            CombatController.Instance.enemies.Add(e);
        }

        CombatController.Instance.InitializeCombat(enemies);
        CameraController.Instance.StartCombatPositionAt(Camera.main.transform.position, targetCameraPosition, transform);
        combatStarted = true;
    }

    void OnDrawGizmos()
    {
        if (debugShowGizmos)
        {
            Gizmos.DrawCube(targetCameraPosition, Vector3.one);
            Gizmos.DrawLine(targetCameraPosition, transform.position);
        }
    }
}
