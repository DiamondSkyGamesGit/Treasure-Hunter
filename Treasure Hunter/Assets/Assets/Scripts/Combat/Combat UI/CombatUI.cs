using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// holds the data for each instance of gui interaction
/// derived classes define their implementation
/// </summary>
public class CombatUI : MonoBehaviour {

    //each instance has their own list of actionButtons
    public List<ActionButton> actionButtons = new List<ActionButton>();

}
