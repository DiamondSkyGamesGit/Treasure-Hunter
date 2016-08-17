using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SkillSystem;

namespace HeroData { 
    /// <summary>
    /// Contains all hero data for the current hero
    /// </summary>
    [RequireComponent(typeof(HeroSkills))]
    [System.Serializable]
    public class Hero : MonoBehaviour, IPausable, IHasActionBar, ITargetable, IDamageable, IDamageDealer {

        private Transform myTransform;
        public Transform MyTransform { get { return myTransform; } set { myTransform = value; } }

        //The Hero Name...
        public string heroName;
        
        //Testing for now, must change later into RPG system Initiative Roll with speed values etc
        //[Range(0, 1f)] public float actionBar = 0f;

        public float speed = 0.1f;

        //think i should do this as an interface with a property or method that heroes and enemies share in some way
        //then maybe controller could call all ICombatEntities to stop their actionBars..?
        public bool actionBarActive = false;
        public bool ActionPaused { get { return actionBarActive; } set { actionBarActive = value; } }

        private ActionBar actionBar;
        public ActionBar MyActionBar{ get { return actionBar; } set { actionBar = value; } }

        private bool isTargetable = true;
        public bool IsTargetable { get { return isTargetable; } set { isTargetable = value; } }

        private bool isTargeted = false;
        public bool IsTargeted { get { return isTargeted; } set { if (IsTargetable) isTargeted = value; else isTargeted = false; } }

        private TargetType _targetType;
        public TargetType targetType { get { return _targetType; } set { _targetType = value; } }

        //--------Events--------------
        public delegate void OnActionBarFull(Hero theHero);
        public event OnActionBarFull onActionBarFull;
        
        //--------State machine-----------

        //the hero must report when it is ready to do something as defined by the state machine
        //dunno if i need this
        public enum ActionState
        {
            ACTION_BAR_CHARGING,
            ACTION_BAR_READY_WAIT_FOR_INPUT,
            ACTION_BAR_READY_QUEUED_ACTION
        }
        //default
        public ActionState actionState = ActionState.ACTION_BAR_CHARGING;

        //The hero's available Skills
        public HeroSkills heroSkills;

        public Skill queuedSkill;

        public bool isGrounded = false;
        public LayerMask groundLayer;

        public bool drawDebugGizmos = false;

        #region //--//-- Monobehaviour Methods --\\--\\

        void Start () {
            MyTransform = GetComponent<Transform>();

            //-- Get Hero Skills --
            if (heroSkills == null) heroSkills = GetComponent<HeroSkills>();

            //-- Setup MyActionBar --
            //NOTE! initialize currentValue to a roll on initiative later, currently Random to spark dynamic combat situation
            MyActionBar = new ActionBar(0f, 1f, UnityEngine.Random.Range(0, 0.3f));

            //-- initialize targetType --
            targetType = TargetType.HERO;
	    }

        void OnEnable()
        {
            //Add myself to GameController active heroes?
            Messenger.AddListener<OnBattleStateChanged>(OnBattleStateChanged);
        }

        void OnDisable()
        {
            Messenger.RemoveListener<OnBattleStateChanged>(OnBattleStateChanged);
        }

        void Update()
        {
            //-- Handle the model being grounded
            //-- ! haven't handled logic check for ISGrounded yet.. fix later
            if (!isGrounded)
            {
                float distanceFromGround = .1f;
                RaycastHit hit;
                if(Physics.Raycast(MyTransform.position, -MyTransform.up, out hit))
                {
                    Vector3 pos = MyTransform.position;
                    pos.y = (hit.point + Vector3.up * distanceFromGround).y;
                    MyTransform.position = pos;
                    //isGrounded = MyTransform.position.y >= 0.01f && MyTransform.position.y < pos.y + (Vector3.up * .2f).y ? true : false;
                }
            }

            //Increment the ActionBar
            if (actionBarActive)
            {
                
                float temp = MyActionBar.CurrentValue < MyActionBar.Max ? MyActionBar.CurrentValue : MyActionBar.Max;
                //float previouValue = MyActionBar.CurrentValue;
                MyActionBar = new ActionBar(0f, 1f, temp + (speed * Time.deltaTime));
               // bool fireOnActionBarFull = temp == MyActionBar.Max;

                //if no player input have been given when action is ready, either
                //do default hero action
                //do last hero action again
                //do queued action
                //if ATB wait, then just pop up the UI menu when a players action is ready
            }
        }

        void OnDrawGizmos()
        {
            if (drawDebugGizmos)
            {
                RaycastHit hit;
                Ray ray = new Ray(MyTransform.position, -MyTransform.up);
                if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
                {
                    Gizmos.DrawLine(ray.origin, hit.point);
                }
            }
        }

        #endregion

        /// <summary>
        /// Handles what to do in this script based on which Battlestate we're in
        /// </summary>
        void OnBattleStateChanged(OnBattleStateChanged battleState)
        {
            switch(battleState.currentBattleState)
            {
                case (BattleState.NORMAL_TIME_FLOW):
                    //do cool stuff
                   // actionState = ActionState.ACTION_BAR_CHARGING;
                    PauseMe(true);
                    break;
                case (BattleState.PAUSE_COMBAT_WAIT_FOR_PLAYER_INPUT):
                    PauseMe(false);
                    break;
            }
        }

        public void UseSkillOnTarget(Skill theSkill, ITargetable target)
        {
            CombatController.Instance.SetCurrentBattleState(BattleState.NORMAL_TIME_FLOW);
            StopCoroutine(QueueAttackExecuteOnActionBarMax(theSkill, target));
            StartCoroutine(QueueAttackExecuteOnActionBarMax(theSkill, target));
        }

        IEnumerator QueueAttackExecuteOnActionBarMax(Skill theSkill, ITargetable target)
        {
            OnQueuedAction queuedAction = new OnQueuedAction();
            queuedAction.theHero = this;
            Messenger.Dispatch(queuedAction);

            while(MyActionBar.CurrentValue < MyActionBar.Max && !ActionPaused)
            {
                yield return null;
            }
            //change execution of damage dealing to be part of the animation later
            if(target != null) {
                OnQueuedActionExecutingNow executingNow = new OnQueuedActionExecutingNow();
                executingNow.theHero = this;
                Messenger.Dispatch(executingNow);

                target.TakeDamage(theSkill.baseDamage + Random.Range(-7, 7));
                StartCoroutine(AwesomeAttackAnimation(theSkill, MyTransform.position, target.MyTransform));
                MyActionBar = new ActionBar(0, 1f, 0f);
            }
            OnQueuedActionExecuted actionExecuted = new OnQueuedActionExecuted();
            actionExecuted.theHero = this;
            Messenger.Dispatch(actionExecuted);
        }

        IEnumerator AwesomeAttackAnimation(Skill theSkill, Vector3 initialPos, Transform target)
        {
            float length = 2f;
            float speed = 6f;
            Vector3 targetPos = target.position + (target.forward * length);
            while(Vector3.Distance(MyTransform.position, targetPos) >= 2.4f)
            {
                MyTransform.position = Vector3.MoveTowards(MyTransform.position, targetPos, Time.deltaTime * speed);
                speed += Time.deltaTime;
                yield return null;
                /*
                curLerpTime += Time.deltaTime;
                if (curLerpTime >= lerpTime)
                    curLerpTime = lerpTime;
                float perc = curLerpTime / lerpTime;
                MyTransform.position = Vector3.Lerp(initialPos, targetPos, perc);
                yield return null;
                */
            }
            speed = 8f;
            while(Vector3.Distance(MyTransform.position, initialPos) > 0.1f)
            {
                MyTransform.position = Vector3.MoveTowards(myTransform.position, initialPos, Time.deltaTime * speed);
                yield return null;
            }
            myTransform.position = initialPos;

        }

        public void ResetActionBar()
        {
            actionBar = new ActionBar(0f, 1f, 0f);
        }
	
        public void PauseMe(bool amIPaused)
        {
            ActionPaused = amIPaused;
        }

        public bool AmIPaused()
        {
            return ActionPaused;
        }

        public void TakeDamage(float damage)
        {
            
        }

        public void TakeDamageAnimation()
        {
            
        }

        public void DealDamage(IDamageable target, float damage)
        {

        }
    }
}
