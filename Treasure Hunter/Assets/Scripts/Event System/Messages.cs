using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HeroData;

public class MyTest : Message
{
    public string myTest;
}

public class OnHealthChanged : Message
{
    public string message;
    public float newHealth;
    public Hero player;

}

public class OnCombatInitialized : Message
{

}

public class OnCombatActiveEnemies : Message
{
    public List<Enemy> activeEnemies;
}

public class OnCombatActiveHeroes : Message
{
    public List<Hero> activeHeroes;
}

public class OnBattleStateChanged : Message
{
    public BattleState previousBattleState;
    public BattleState currentBattleState;
}
