using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstantiationTest : MonoBehaviour {

    public ActionButton clone;
    public List<ActionButton> buttons = new List<ActionButton>();
    public ScrollableActionList actionList;
	void Start () {

        // ActionButton g = Instantiate(clone) as ActionButton;
       // actionList.InstantiateAndDisplayItems(buttons);

	}
	
	void Update () {
	
	}
}
