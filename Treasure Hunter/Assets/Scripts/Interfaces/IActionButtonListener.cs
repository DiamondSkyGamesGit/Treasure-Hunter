using UnityEngine;
using System.Collections;

public interface IActionButtonListener {

    void OnActionButtonClicked(ActionButton theButton, ITargetable theTarget);
}
