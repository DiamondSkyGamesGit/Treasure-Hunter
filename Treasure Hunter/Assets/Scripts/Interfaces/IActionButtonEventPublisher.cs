using UnityEngine;
using System.Collections;

public delegate void OnActionButtonClick(ActionButton theActionButton, ITargetable theTarget);

public interface IActionButtonEventPublisher {

    event OnActionButtonClick onActionButtonClick;
}
