using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HeroData;
using System.Linq;

/// <summary>
/// Holds all important data that scripts need like active heroes, gameData
/// Update as this script grows..
/// </summary>
public class GameController : MonoBehaviour {

    public static GameController Instance = null;

    //All available heroes in the game @ time in the game or overall, dunno yet
    public List<Hero> availableHeroes = new List<Hero>();
    //The current active heroes. Let's follow FF with up to 3 party members
    public List<Hero> activeHeroes = new List<Hero>();


    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


	void Start () {

        //Finds heroes in scene if theyre not registered in inspector, should change this later to who is selected as active from menu
        //or heroes register themselves with GameController <3
        if (activeHeroes.Count <= 0)
        {
            Hero[] temp = FindObjectsOfType<Hero>();
            activeHeroes = temp.ToList();
        }
	}

   public void AddHeroToActiveHeroes(Hero theHero)
    {
        if (activeHeroes.Count < 3 && !activeHeroes.Contains<Hero>(theHero))
            activeHeroes.Add(theHero);
        else
            Debug.LogWarning("ActiveHeroes are " + activeHeroes.Count + "! you cannot add more than 3 players active at one time!");
    }

    public void RemoveHeroFromActiveHeroes(Hero theHero)
    {
        foreach(Hero h in activeHeroes)
        {
            if (h.heroName == theHero.heroName)
                activeHeroes.Remove(h);
        }
    }


	
}
