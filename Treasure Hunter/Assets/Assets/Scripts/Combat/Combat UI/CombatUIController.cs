using UnityEngine;
using System.Collections;


/// <summary>
/// Handles control of which UI elements to show for each instance of ScrollableActionList at a given time
/// listens to relevant combat events
/// public scripts should be added in inspector
/// </summary>
public class CombatUIController : MonoBehaviour {
    
    public ScrollableActionList scrollableActionList;
    public ActionButton defaultEnemyActionButton;

    public CombatUIOffensiveActions combatUIOffensiveActions;
    public CombatUiTargetSelection combatUITargetSelection;

	// Use this for initialization
	void Start () {
	
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DisplayOffensiveActions()
    {

    }

    void DisplayTargetSelection()
    {
        //scrollableActionList.DisplayActions<ActionButton>(combatUITargetSelection.activeEnemies);
    }
}
