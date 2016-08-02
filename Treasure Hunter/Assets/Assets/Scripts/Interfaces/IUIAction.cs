using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public interface IUIAction  {

    //an ui action must implement the following
    //the action that this uiaction can do
    //a uiAction contains a button
    //void OnClick();

    void DoAction();
}
