using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HeroData;

public class CombatActionBar : MonoBehaviour {

    public Hero owner;
    /// <summary>
    /// Must be set in inspector to 0, 1, 2, as to get the player from activeHeroes.
    /// upper hero or leftmost hero is 0, then 1, then 2
    /// </summary>
    [Tooltip("Must be set in inspector to 0, 1, 2 in order")]
    public int ownerIndex = -1;
    public float ActionBarValue { get { return owner != null ? owner.actionBar : 0; } }
    public bool ActionBarActive { get { return owner != null ? owner.actionBarActive : false; } }
    public Image actionBar;
    
	// Use this for initialization
	void Start ()
    {
        if (actionBar == null) actionBar = GetComponent<Image>();
        if (actionBar.type != Image.Type.Filled) { 
            actionBar.type = Image.Type.Filled;
            actionBar.fillMethod = Image.FillMethod.Horizontal;
        }

        if (owner == null && ownerIndex != -1) owner = GameController.Instance.activeHeroes[ownerIndex];
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ActionBarActive)
        {
            actionBar.fillAmount = ActionBarValue;
        }
	}
}
