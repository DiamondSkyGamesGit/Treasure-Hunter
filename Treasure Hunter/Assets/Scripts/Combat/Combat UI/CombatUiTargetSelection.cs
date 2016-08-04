using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Gets a list of all active enemies
/// and displays them using ScrollableActionList
/// </summary>
public class CombatUiTargetSelection : CombatUI {

    public ScrollableActionList scrollableActionList;
    public SelectTargetButton actionButtonPrefab;
    public List<Enemy> activeEnemies = new List<Enemy>();


    void OnEnable()
    {
        CombatController.Instance.onCombatActiveEnemies += OnCombatGetActiveEnemies;
    }

    void OnDisable()
    {
        CombatController.Instance.onCombatActiveEnemies -= OnCombatGetActiveEnemies;
    }

    void OnCombatGetActiveEnemies(List<Enemy> enemies)
    {
        activeEnemies = enemies;
        //foreach enemy, instantiate actionButtonPrefab and set text + 
        for(int i = 0; i < activeEnemies.Count; i++)
        {
            SelectTargetButton s = Instantiate<SelectTargetButton>(actionButtonPrefab);
            actionButtons.Add(s);
            s.myButton.GetComponentInChildren<Text>().text = activeEnemies[i].EnemyName + " " + i;
            s.myLayoutElement.minHeight = 60f;
            s.myTarget = activeEnemies[i];
            
        }
        scrollableActionList.DisplayActions<ActionButton>(actionButtons);
        
    }

}
