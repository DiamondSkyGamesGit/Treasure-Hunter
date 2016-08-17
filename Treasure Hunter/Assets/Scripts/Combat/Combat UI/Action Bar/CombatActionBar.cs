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
    //The current ActionBarValue as given by owner
    public float ActionBarValue { get { return owner != null ? owner.MyActionBar.CurrentValue : 0; } }
    //Is the ActionBarActive as given by owner
    public bool ActionBarActive { get { return owner != null ? owner.actionBarActive : false; } }
    //The image that represents actionBarValue from 0 - 1
    public Image actionBarImage;
    
	// Use this for initialization
	void Start ()
    {

        if (actionBarImage == null) actionBarImage = GetComponent<Image>();
        if (actionBarImage.type != Image.Type.Filled) {
            actionBarImage.type = Image.Type.Filled;
            actionBarImage.fillMethod = Image.FillMethod.Horizontal;
        }

        if (owner == null && ownerIndex != -1) owner = GameController.Instance.activeHeroes[ownerIndex];

    }

    void OnEnable()
    {
        Messenger.AddListener<OnQueuedAction>(OnQueuedAction);
        Messenger.AddListener<OnQueuedActionExecuted>(OnQueuedActionExecuted);
    }
    void OnDisable()
    {
        Messenger.RemoveListener<OnQueuedAction>(OnQueuedAction);
        Messenger.RemoveListener<OnQueuedActionExecuted>(OnQueuedActionExecuted);
    }

    void OnQueuedAction(OnQueuedAction action)
    {
        if(action.theHero.heroName == owner.heroName)
            actionBarImage.color = Color.cyan;
    }

    void OnQueuedActionExecuted(OnQueuedActionExecuted action)
    {
        if (action.theHero.heroName == owner.heroName)
            actionBarImage.color = Color.white;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ActionBarActive)
        {
            actionBarImage.fillAmount = ActionBarValue;
        }
	}
}
