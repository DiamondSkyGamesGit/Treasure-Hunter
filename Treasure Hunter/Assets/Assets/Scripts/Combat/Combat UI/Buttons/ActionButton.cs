using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using SkillSystem;

[System.Serializable]
public abstract class ActionButton : MonoBehaviour, IUIAction {

    public Button myButton;
    public LayoutElement myLayoutElement;



    public ActionButton()
    {
        Start();
    }

	// Use this for initialization
	void Start () {
        

	}

    protected virtual void OnEnable()
    {
        if (myButton == null) { 
            myButton = GetComponent<Button>();
            if(myButton.onClick == null)
                myButton.onClick.AddListener(() => DoAction() );
        }

        if (myLayoutElement == null)
            myLayoutElement = GetComponent<LayoutElement>();
    }

   protected virtual void OnDisable()
    {

    }

    protected abstract void AddButtonListener();
    protected abstract void RemoveButtonListener();

    public abstract void DoAction();
    public abstract void DoAction(Skill skillToUse);

}
