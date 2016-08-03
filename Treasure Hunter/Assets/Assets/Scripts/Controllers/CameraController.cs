using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public static CameraController Instance = null;
    public Vector3 defaultCameraPosition;
    public Vector3 targetCombatPosition;
    public Vector3 targetRotationEuler;
    public Vector3 defaultCameraRotation;
    public float lerpTime = 2f;

    private float currentLerpTime;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }



	// Use this for initialization
	void Start () {
        defaultCameraPosition = transform.position;
        defaultCameraRotation = transform.localEulerAngles;
	}



    public void StartCombatPosition()
    {
        //StartCoroutine(LerpToPosition(transform.position, transform.localEulerAngles));
    }

    public void StartCombatPositionAt(Vector3 pos)
    {
        StartCoroutine(LerpToPosition(pos));
    }

    public void StartCombatPositionAt(Vector3 startPosition, Vector3 targetPosition, Transform lookAtTarget)
    {
        StartCoroutine(LerpToPosition(startPosition, targetPosition, transform));
    }

    /// <summary>
    /// FIX this
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    IEnumerator LerpToPosition(Vector3 startPos)
    { 
        while(transform.position != targetCombatPosition)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startPos, targetCombatPosition, perc);
            yield return null;
        }
        currentLerpTime = 0;
    }

    IEnumerator LerpToPosition(Vector3 startPos, Vector3 targetPos, Transform lookAtTarget)
    {
        while (transform.position != targetPos)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startPos, targetPos, perc);
            transform.LookAt(transform, Vector3.up);
            yield return null;
        }
        currentLerpTime = 0;

    }

    IEnumerator LerpToPosition(Vector3 startPos,bool unused, Vector3 startRot)
    {
        while (transform.position != targetCombatPosition)
        {
            currentLerpTime += Time.deltaTime;
            float perc = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startPos, targetCombatPosition, perc);
            transform.localEulerAngles = Vector3.Slerp(startRot, targetRotationEuler, perc);
            yield return null;
        }
        currentLerpTime = 0;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
