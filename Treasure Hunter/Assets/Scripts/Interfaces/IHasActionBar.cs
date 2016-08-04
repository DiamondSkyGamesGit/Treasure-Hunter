using UnityEngine;
using System.Collections;

/// <summary>
/// Defines an ActionBar
/// </summary>
[System.Serializable]
public struct ActionBar
{
    private float min;
    public float Min { get { return min; } private set { min = value; } }
    private float max;
    public float Max { get { return max; } private set { max = value; } }
    private float currentValue;
    public float CurrentValue { get { return currentValue; } set { currentValue = value; } }

    public ActionBar(float _min, float _max, float _currentValue)
    {
        min= _min;
        max = _max;
        if (_currentValue <= max) currentValue = _currentValue;
        else currentValue = max;
    }
}

public interface IHasActionBar {

     ActionBar MyActionBar { get; set; }
}
