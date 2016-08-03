using UnityEngine;
using System.Collections;

public interface IPausable  {

    //pausable objects will have a property which in turn should point to their local way of pausing their state
    bool ActionPaused { get; set; }
    //an object must implement their own way of pausing their state through this method
    void PauseMe(bool amIPaused);
    bool AmIPaused();
}
