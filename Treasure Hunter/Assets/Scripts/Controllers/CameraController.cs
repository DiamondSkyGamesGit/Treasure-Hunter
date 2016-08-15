using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour {

    public static CameraController Instance = null;
    public Vector3 defaultCameraPosition;
    public Vector3 targetCombatPosition;
    public Vector3 targetRotationEuler;
    public Vector3 defaultCameraRotation;
    public float lerpTime = 2f;

    public Bloom bloomEffect;

    private float currentLerpTime;
    private bool combatIntroEffectPlayed = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (bloomEffect == null) bloomEffect = GetComponent<Bloom>();
    }


	// Use this for initialization
	void Start () {
        defaultCameraPosition = transform.position;
        defaultCameraRotation = transform.localEulerAngles;

        Messenger.AddListener<OnBattleStateChanged>(OnBattleStateChanged);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {
        Messenger.RemoveListener<OnBattleStateChanged>(OnBattleStateChanged); 
    }

    void OnBattleStateChanged(OnBattleStateChanged newBattleState)
    {
        switch (newBattleState.currentBattleState) { 
            case (BattleState.COMBAT_INTRODUCTION):
                if(!combatIntroEffectPlayed)
                    StartCoroutine(CombatIntroductionImageBloomEffect(1f, 30f));
                break;
            case BattleState.EXIT_COMBAT:
                combatIntroEffectPlayed = false;
                break;
        }
    }

    IEnumerator CombatIntroductionImageBloomEffect(float lerpTime, float startBloomIntensity)
    {
        float curLerpTime = 0;
        float targetBloomIntensity = bloomEffect.bloomIntensity;
        bloomEffect.bloomIntensity = startBloomIntensity;
        
        //Descending value from high to low
        while (bloomEffect.bloomIntensity > targetBloomIntensity)
        {
            curLerpTime += Time.deltaTime;
            float perc = curLerpTime / lerpTime;
            
            bloomEffect.bloomIntensity = Mathf.Lerp(startBloomIntensity, targetBloomIntensity, perc);
            yield return null;
        }
        curLerpTime = 0;
        bloomEffect.bloomIntensity = targetBloomIntensity;
        combatIntroEffectPlayed = true;
    }

    #region --Combat Position Methods--

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
    #endregion

    // Update is called once per frame
    void Update () {
	
	}
}
