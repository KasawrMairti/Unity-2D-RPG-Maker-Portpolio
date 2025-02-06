using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour, IDamageble
{
    [SerializeField] private GameObject attackParticle;

    [SerializeField] private List<RuntimeAnimatorController> animatorController = new();
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public NavMeshAgent navMeshAgent { get; private set; }

    private ColliderSensor colliderAttackRange;
    private ColliderSensor colliderAttackSensor;

    private bool Is_Died;

    private bool Is_Canvas;
    private Vector3 movePoint;
    private Vector3 moveLastPoint;
    private Vector3 moveOrderPoint;
    private Vector2 attackDir;

    private float touchTime;

    public PlayerStatus playerStatus;

    private bool Is_Order;
    private bool Is_AttackOrdered;
    private float f_attackCoolTime;
    private float f_attackCoolTimeMax;
    private float f_damagedCoolTime;
    private float f_damagedCoolTimeMax;

    public bool b_IsDamageble() { return false; }

    private void Awake()
    {
        ObjectManager.Add("Player", gameObject);

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        colliderAttackRange = gameObject.transform.Find("AttackRange").GetComponent<ColliderSensor>();
        colliderAttackSensor = gameObject.transform.Find("AttackSensor").GetComponent<ColliderSensor>();

        Is_Canvas = false;
        movePoint = transform.position;
        moveLastPoint = transform.position;
        moveOrderPoint = transform.position;
        attackDir = Vector2.zero;

        touchTime = 0.0f;

        Is_Order = false;
        Is_AttackOrdered = false;
        f_attackCoolTime = 0.0f;
        f_attackCoolTimeMax = 1.0f;
        f_damagedCoolTime = 0.0f;
        f_damagedCoolTimeMax = 0.5f;

        Is_Died = false;
    }

    private void Start()
    {
        playerStatus = UserDataManager.Instance.GetPlayerStatus();
    }

    private void OnEnable()
    {
        StartCoroutine(EventMove());
        StartCoroutine(MoveOrder());
        StartCoroutine(MoveAlorithm());
    }

    private void Update()
    {
        UIManager.Instance.sliderHP.value =  playerStatus.Hp /  playerStatus.HpMax;
        UIManager.Instance.sliderMP.value =  playerStatus.Mp /  playerStatus.MpMax;
        UIManager.Instance.sliderExP.value = playerStatus.Exp / playerStatus.ExpMax;

        transform.position = new(transform.position.x, transform.position.y, transform.position.y / 100f);

        if (f_damagedCoolTime < f_damagedCoolTimeMax) f_damagedCoolTime += Time.deltaTime;
        if (f_attackCoolTime < f_attackCoolTimeMax) f_attackCoolTime += Time.deltaTime;


        if (Is_Died) return;

        AnimationUpdate();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.runtimeAnimatorController = animatorController[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.runtimeAnimatorController = animatorController[1];
        }
    }

    private void AnimationUpdate()
    {
        animator.SetFloat("Horizontal", navMeshAgent.velocity.x);
        animator.SetFloat("Vertical", navMeshAgent.velocity.y);

        if (navMeshAgent.velocity.x != 0 || navMeshAgent.velocity.y != 0)
        {
            animator.SetFloat("LastHorizontal", navMeshAgent.velocity.x);
            animator.SetFloat("LastVertical", navMeshAgent.velocity.y);

            if (navMeshAgent.velocity.x > 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
        }
    }

    private IEnumerator EventMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => !Is_Died);

            yield return new WaitUntil(() => Is_Order);

            moveLastPoint = moveOrderPoint;
            Move();
        }
    }

    private IEnumerator MoveOrder()
    { 
        while (true)
        {
            yield return new WaitUntil(() => !Is_Died);

            yield return new WaitUntil(() => SystemManager.isControl == SystemManager.Control.CONTROL && !Is_Order);
            
            if (EventSystem.current.IsPointerOverGameObject()) Is_Canvas = true;
            else Is_Canvas = false;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    Is_Canvas = true;
            else Is_Canvas = false;

            if (!Is_Canvas)
            {
                if (SystemManager.Instance.Is_Touched)
                {
                    if (touchTime < 0.1f) touchTime += Time.deltaTime;
                    else
                    {
                        movePoint = SystemManager.Instance.moveMousePoint;
                        moveLastPoint = movePoint;
                    }
                }
                else
                {
                    if (touchTime < 0.1f && touchTime > 0)
                    {
                        movePoint = SystemManager.Instance.moveMousePoint;
                        moveLastPoint = movePoint;
                    }

                    touchTime = 0.0f;
                }
            }
        }
    }

    private IEnumerator MoveAlorithm()
    {
        while (true)
        {
            yield return new WaitUntil(() => !Is_Died);

            yield return new WaitUntil(() => SystemManager.isControl == SystemManager.Control.CONTROL && !Is_Order);

            if (!SystemManager.Instance.Is_Touched && colliderAttackSensor.b_IsTarget)
            {
                if (colliderAttackRange.b_IsTarget)
                {
                    navMeshAgent.SetDestination(new Vector3(colliderAttackRange.v_targetPosition.x, colliderAttackRange.v_targetPosition.y, transform.position.z));
                    Attack();
                }
                else
                {
                    if (f_attackCoolTime >= f_attackCoolTimeMax)
                    {
                        navMeshAgent.isStopped = false;
                        navMeshAgent.SetDestination(new Vector3(colliderAttackSensor.v_targetPosition.x, colliderAttackSensor.v_targetPosition.y, transform.position.z));
                        f_attackCoolTime = 0.0f;
                    }
                }
            }
            else 
            { 
                if (f_attackCoolTime >= f_attackCoolTimeMax && (touchTime <= 0 || touchTime > 0.1f)) Move(); 
            }
        }
    }

    private void Move()
    {
        navMeshAgent.isStopped = false;

        if (navMeshAgent.SetDestination(new Vector3(moveLastPoint.x, moveLastPoint.y, transform.position.z)) && !navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (Is_Order) Is_Order = false;
                }
            }
        }
    }

    private void Attack()
    {
        navMeshAgent.isStopped = true;

        if (f_attackCoolTime >= f_attackCoolTimeMax)
        {
            if (Is_AttackOrdered) return;

            f_attackCoolTime = 0.0f;

            attackDir = colliderAttackRange.v_targetPosition - (Vector2)transform.position;
            animator.SetFloat("LastHorizontal", attackDir.x);
            animator.SetFloat("LastVertical", attackDir.y);
            if (attackDir.x > 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;

            animator.SetTrigger("Attack");

        }
    }

    public void AttackEffect()
    {
        if (!Is_AttackOrdered)
        {
            Is_AttackOrdered = true;

            float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
            if (angle >= 135.0f || angle < -135.0f) AttackAnimation(0);
            else if (angle >= 45.0f && angle < 135) AttackAnimation(1);
            else if (angle >= -45.0f && angle < 45.0f) AttackAnimation(2);
            else if (angle >= -135.0f && angle < -45.0f) AttackAnimation(3);
        }

    }

    public void AttackAnimation(int dir)
    {
        Collider2D[] colliders;
        Vector2 center;
        GameObject particle = Instantiate(attackParticle);
        switch (dir)
        {
            case 0:
                // 왼쪽 범위 공격 주기
                center = (Vector2)transform.position + new Vector2(-0.675f, 0.5f);
                colliders = Physics2D.OverlapBoxAll(center, new Vector2(1.25f, 2.5f), 0.0F);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.TryGetComponent<IDamageble>(out var monster))
                    {
                        Hit(monster);
                    }
                }
                particle.transform.position = center;
                break;

            case 1:
                // 위쪽 범위 공격 주기
                center = (Vector2)transform.position + new Vector2(0.0f, 1.25f);
                colliders = Physics2D.OverlapBoxAll(center, new Vector2(2.5f, 1.5f), 0.0F);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.TryGetComponent<IDamageble>(out var monster))
                    {
                        Hit(monster);
                    }
                }
                particle.transform.position = center;
                break;

            case 2:
                // 오른쪽쪽 범위 공격 주기
                center = (Vector2)transform.position + new Vector2(0.675f, 0.5f);
                colliders = Physics2D.OverlapBoxAll(center, new Vector2(1.25f, 2.5f), 0.0F);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.TryGetComponent<IDamageble>(out var monster))
                    {
                        Hit(monster);
                    }
                }
                particle.transform.position = center;
                break;

            case 3:
                // 아래쪽 범위 공격 주기
                center = (Vector2)transform.position + new Vector2(0.0f, -0.25f);
                colliders = Physics2D.OverlapBoxAll(center, new Vector2(2.5f, 1.5f), 0.0F);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.TryGetComponent<IDamageble>(out var monster))
                    {
                        Hit(monster);
                    }
                }
                particle.transform.localScale = new Vector3(particle.transform.localScale.x * -1.0f, particle.transform.localScale.y * -1.0f, particle.transform.localScale.z);
                particle.transform.position = center;
                break;
        }
        if (!spriteRenderer.flipX)
            particle.transform.localScale = new Vector3(particle.transform.localScale.x * -1.0f, particle.transform.localScale.y, particle.transform.localScale.z);
        particle.transform.localScale *= 0.25f;
        Destroy(particle, 0.75f);
    }

    public void AttackEndEvent() // 애니메이션 이벤트에 호출됨
    {
        Is_AttackOrdered = false;
    }

    public void Damaged(float life)
    {
        playerStatus.Hp -= life;

        if (f_attackCoolTime < f_attackCoolTimeMax && playerStatus.Hp > 0)
        {
            if (f_damagedCoolTime >= f_damagedCoolTimeMax)
            {
                animator.SetTrigger("Damaged");
                f_damagedCoolTime = 0f;
            }
        }

        if (playerStatus.Hp <= 0) animator.SetTrigger("Die");
    }

    public void Hit(IDamageble target)
    {
        target.Damaged(1.0f);

    }

    public void Order(Vector2 pos)
    {
        Is_Order = true;
        moveOrderPoint = pos;

        navMeshAgent.enabled = false;
        navMeshAgent.enabled = true;
        SystemManager.Instance.virtualCamera.Follow = transform;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = SystemManager.Instance.moveMousePoint;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0 || EventSystem.current.IsPointerOverGameObject();
    }
}
