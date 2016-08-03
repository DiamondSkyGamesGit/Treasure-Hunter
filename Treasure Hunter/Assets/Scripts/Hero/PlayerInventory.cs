using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour {

    public static PlayerInventory Instance = null;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	void Start () {
	
	}
	
	void Update () {
	
	}
}
