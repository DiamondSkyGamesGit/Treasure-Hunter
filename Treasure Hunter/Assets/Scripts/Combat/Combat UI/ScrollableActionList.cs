using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Base class for all types of scrollableLists, used in UI.
/// Derived classes must define what kind of items is in list and what interactions each derived class do with the list
/// Could maybe be an interface
/// </summary>
public class ScrollableActionList : MonoBehaviour {

    public List<ActionButton> actionButtons = new List<ActionButton>();
    public RectTransform contentArea;

    // Use this for initialization
    void Start () {
	
	}

    void OnEnable()
    {
        Messenger.AddListener<OnCombatUIDisplayActionButtons>(OnCombatUIDisplayActionButtons);

    }

    void OnDisable()
    {
        DestroyActionButtons();
        actionButtons.Clear();
        Messenger.RemoveListener<OnCombatUIDisplayActionButtons>(OnCombatUIDisplayActionButtons);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Display the incoming buttons from OnCombatUIDisplayActionButtons in my list by setting their parent
    /// </summary>
    void OnCombatUIDisplayActionButtons(OnCombatUIDisplayActionButtons data)
    {
        //-- Destroy my current actionButtons before adding new ones
        DestroyActionButtons();
        contentArea.localPosition = Vector3.zero;

        for(int i = 0; i < data.actionButtons.Count; i++)
        {
            data.actionButtons[i].transform.SetParent(contentArea.transform, false);
            //-- Add to my list
            actionButtons.Add(data.actionButtons[i]);
        }
    }

    public void DestroyActionButtons()
    {
        if (actionButtons.Count > 0)
        {
            foreach (var g in actionButtons)
                Destroy(g.gameObject);

            actionButtons.Clear();
        }
        else
        {
            Debug.LogWarning("No actionButtons in List");
            return;
        }
    }

}
