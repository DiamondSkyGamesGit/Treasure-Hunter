using UnityEngine;
using System.Collections;

public delegate void OnActionButtonClick(ActionButton theActionButton);

public interface IActionButtonEventPublisher {

    event OnActionButtonClick onActionButtonClick;
}
