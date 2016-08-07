﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;
using HeroData;
using System;

public class MyEventManager : MonoBehaviour {

    public static MyEventManager Instance = null;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    public delegate void OnCombatActiveHeroes(List<Hero> activeHeroes);
    public event OnCombatActiveHeroes onCombatActiveHeroes;
    public void FireOnCombatActiveHeroes(List<Hero> activeHeroes) { onCombatActiveHeroes(activeHeroes); }

    //if hero party changes during combat this must be fired to subs. Ensure to desubscribe listeners to the previous active heroes list
    public delegate void OnCombatActiveHeroesChanged(List<Hero> previousActiveHeroes, List<Hero> activeHeroes);
    public event OnCombatActiveHeroesChanged onCombatActiveHeroesChanged;
    public void FireOnCombatActiveHeroesChanged(List<Hero> previousActiveHeroes, List<Hero> activeHeroes) { onCombatActiveHeroesChanged(previousActiveHeroes, activeHeroes); }

    //fired whenever amount of enemies change as well
    public delegate void OnCombatActiveEnemies(List<Enemy> enemyList);
    public event OnCombatActiveEnemies onCombatActiveEnemies;

    public delegate void OnCombatInitiated();
    public event OnCombatInitiated onCombatInitiated;

    public delegate void OnBattleStateChanged(BattleState combatState);
    public event OnBattleStateChanged onBattleStateChanged;

    // Use this for initialization
    void Start () {



	}


}
