using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class MyDebugger : MonoBehaviour {

    /*
    void OnTriggerEnter(Collider other)
    {
        var temp = other.GetType().GetInterfaces().ToList();
        foreach(var v in temp)
        {
            if(v is IDamageable)
            {
                var objectToDamage = v as IDamageable;
                objectToDamage.TakeDamage(20f);
            }
        }
        */


    //temp = other.get

    OnHealthChanged onHealthChanged = new OnHealthChanged();
    
    
    void Awake()
    {
        Messenger.AddListener<MyTest>(GetMessage);
        MyTest test = new MyTest();
        test.myTest = "EEEEEEEEEEEGGGGGG";
        Messenger.Dispatch(test);
    }

	void Start () {

        Messenger.AddListener<OnHealthChanged>(MyOnHealthChanged);
        onHealthChanged.message = "Yo! Sender denne eventen";
        onHealthChanged.newHealth = 100f;
        onHealthChanged.player = GameController.Instance.activeHeroes[0];
        Messenger.Dispatch(onHealthChanged);

	}

    OnHealthChanged SetMessageOnHealthChangedData(string message, float newHealth, HeroData.Hero player)
    {
        OnHealthChanged temp = new OnHealthChanged();
        temp.message = message;
        temp.newHealth = newHealth;
        temp.player = player;
        return temp;
    }

    void MyOnHealthChanged(OnHealthChanged healthChange)
    {
        Debug.Log(healthChange.message + " " + healthChange.newHealth);
    }


	void Update () {
	
	}

    void GetMessage(MyTest testObj)
    {
        Debug.Log(testObj.myTest);
    }
}
