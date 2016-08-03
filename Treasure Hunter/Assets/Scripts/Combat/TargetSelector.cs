using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the visual representation of which target is selected in combat
/// Might want to use this script to allow for several instances of target selector in the case of multiple enemies being selected
/// </summary>
public class TargetSelector : MonoBehaviour {

    public GameObject targetSelectorPrefab;
    private GameObject clone;
    public Transform selectedTargetTransform;
    public Vector3 positionOffset;

    public void SelectTarget(Transform target)
    {
        if(clone == null)
            clone = Instantiate(targetSelectorPrefab);
        positionOffset = new Vector3(0, target.localScale.y + 1f, 0);
        clone.transform.position = target.position + positionOffset;
    }

    public void DestroySelector()
    {
        Destroy(clone);
    }
}
