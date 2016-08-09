
//global definition of BattleState State machine
public enum BattleState
{
    NOT_COMBAT,
    COMBAT_INTRODUCTION,
    NORMAL_TIME_FLOW,
    PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT,
    PLAYER_WIN,
    PLAYER_LOOSE,

}

//The selected Player Action when dealing with Combat UI
public enum SelectedAction
{
    //-- Not Combat : Turn this as default when not in combat?
    //-- Not_Selected_Yet...: Enable UI, Display default skill selection GUI buttons
    //-- Select_Skill_To_Use: Choose a skill to use from a list displayed in it's own larger content pane
    //-- Select_Target: Display possible targets
    //-- Selected_target_Enemy & Friendly: Not sure yet if this stateMachine needs it
    //-- Flee: The player is trying to flee the combat
    //-- Change active hero: The player changes which hero is "active" in the GUI


    NOT_COMBAT,
    NOT_SELECTED_YET_DISPLAY_DEFAULT_ACTIONS,
    SELECT_SKILL_TO_USE,
    SELECT_TARGET,
    SELECTED_TARGET_ENEMY,
    SELECTED_TARGET_FRIENDLY,
    FLEE,
    CHANGE_ACTIVE_HERO
}

public enum CombatUIScrollDirection
{
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3
}

/// <summary>
/// Add to this as skills grow?
/// </summary>
public enum SkillType
{
    ATTACK,
    MAGIC,
    ITEM
}

