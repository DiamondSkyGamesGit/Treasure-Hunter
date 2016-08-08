
//global definition of BattleState State machine
public enum BattleState
{
    NOT_COMBAT,
    COMBAT_INTRODUCTION,
    NORMAL_TIME_FLOW,
    PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT,
    PLAYER_WIN,
    PLAYER_LOOSE
}

//The selected Player Action when dealing with Combat UI
public enum SelectedAction
{
    NOT_SELECTED_YET,
    ATTACK,
    MAGIC,
    ITEM,
    SELECT_TARGET,
    SELECTED_TARGET_ENEMY,
    SELECTED_TARGET_FRIENDLY
}

