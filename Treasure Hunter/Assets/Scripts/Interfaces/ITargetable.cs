using UnityEngine;
using System.Collections;

public enum TargetType
{
    HERO,
    ENEMY,
    NEUTRAL
}

public interface ITargetable : IDamageable {

    //All implementers of this script must have the possibility to be targeted
    //what does that mean?
    //a targetable can be targeted by UI, Input or something
    //I want to send an event to all subs which button was clicked and what target that button is responsible for
    //so that later, listeners can implement something to do with that target, and possibly the button
    //a button might also have the sideEffect of CanTargetHeroes?
    bool IsTargetable { get; set; }
    TargetType targetType { get; set; }
}
