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
    void Awake()
    {
        Messenger.AddListener<MyTest>(GetMessage);
        MyTest test = new MyTest();
        test.myTest = "EEEEEEEEEEEGGGGGG";
        Messenger.Dispatch(test);
    }

	void Start () {
        
	}

	void Update () {
	
	}

    void GetMessage(MyTest testObj)
    {
        Debug.Log(testObj.myTest);
    }
}
