using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HeroData;
using SkillSystem;


public class OnCombatInitialized : Message
{
    //-- Empty for now
}

/// <summary>
/// Dispatched when List of Enemies active in battle at current time is changed
/// </summary>
public class OnCombatActiveEnemies : Message
{
    public List<Enemy> activeEnemies;
}

/// <summary>
/// Dispatched when List of Heroes active in battle at current time is changed
/// Should be considered as the active party
/// </summary>
public class OnCombatActiveHeroes : Message
{
    public List<Hero> activeHeroes;
}

/// <summary>
/// Dispatched when the BattleState statemachine changes during combat to handle pausing of characters, display of GUI etc at a certain state
/// </summary>
public class OnBattleStateChanged : Message
{
    public BattleState previousBattleState;
    public BattleState currentBattleState;
}



/// <summary>
/// Dispatched when an "ActionButton" is clicked in the Combat GUI. 
/// Variables may be --Null-- when dispatched! Listener must check for the information they expect depending on context
/// </summary>
public class OnActionButtonClick : Message
{
    public ActionButton.ActionButtonType actionButtonType;
    public ITargetable target;
    public Skill theSkill;
    //Add more here?
}

/// <summary>
/// Dispatched when the Active hero changes in Combat GUI.
/// The active Hero is the character whose actions are represented in the GUI
/// The player may choose which Hero is Active in GUI by pressing Left or Right on DPAD / keyboard
/// </summary>
public class OnCombatUIActiveHero :Message
{
    public Hero theActiveHero;
}

public class OnCombatUIChangeActiveHero : Message
{
    public CombatUIScrollDirection btnDirectionOnInput;
}


/// <summary>
/// Dispatched when ActionButtons are ready to be instantiated and placed in the GUI.
/// Dispatchers instantiate the buttons in their scripts, listeners position them in their relevant position
/// </summary>
public class OnCombatUIDisplayActionButtons : Message
{
    public List<ActionButton> actionButtons;
}

/// <summary>
/// Dispatched when a player has chosen an Action in the Combat GUI.
/// Delivers the previous and the current selected action.
/// Listeners implement behaviors based on what they want to do when selected action is of a type
/// </summary>
public class OnCombatUISelectedAction : Message
{
    //expose State Machine
    public SelectedAction previousSelectedAction;
    public SelectedAction selectedAction;
}

/// <summary>
/// Dispatched when the player has chosen a skill to use.
/// Targeting of the skill is dispatched in another Message based on SelectedAction state machine
/// </summary>
public class OnCombatUISkillSelected : Message
{
    public Skill theSkill;
}

/// <summary>
/// Dispatched when the player has chosen a target.
/// The Skill to use 
/// </summary>
public class OnCOmbatUITargetSelected : Message
{
    //The message must contain the actual reference to the target, ideally Enemy & Hero
    public TargetType targetType;
    //The reciever may cast the ITargetable to what he needs based on knowing the targetType
}

public class OnSkillUseOnTarget : Message
{
    //-- Delivers the skill chosen in GUI to reciever
    //-- Delivers the target
    //-- Should be final state in an interaction chain before animation and damage application

    //-- The skill to use on target
    public Skill theSkill;
    //-- The target. Enemy and Hero implements ITargetable for now. Cast in reciever
    public ITargetable theTarget;
}

public class OnEnemyDie : Message
{
    public Enemy theEnemy;
}




