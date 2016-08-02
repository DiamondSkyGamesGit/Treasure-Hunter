using UnityEngine;
using System.Collections;

public interface IPhysicalSkill  {
    
    //-------Properties----------

    //a physical skill needs a target, though i don't think i need it in the contract, maybe through targeting scripts down the line this is sent right
    //it deals damage of (type) physical
    //it has base damage
    float BaseDamage { get; }
    //it has a charge time
    float CastTime { get; }
    //it has damage modifiers (Implement later, should be it's own class or something)


    //-------Methods-------------
    
}
