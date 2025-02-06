using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour, IDamageble
{
    protected bool b_IsActive = false;
    public bool b_IsDamageble() { return true; }

    protected enum ACT { IDLE, MOVE, ATTACK, DIED }
    protected ACT act = ACT.IDLE;

    [SerializeField] protected EnemyBullet enemyBulletOBJ;
    protected List<EnemyBullet> enemyBullets = new List<EnemyBullet>();

    protected MonsterStatus status;

    protected BoxCollider2D collider2D;
    protected ColliderSensor colliderSensorMove;
    protected ColliderSensor colliderSensorAttack;
    protected Animator anim;

    protected NavMeshAgent navMeshAgent;
    protected Vector2 playerPosition;

    protected bool Is_Order = false;
    protected Vector2 orderPosition;

    protected abstract void StatusInitialize();

    protected virtual void Awake()
    {
        collider2D = gameObject.transform.Find("Image").GetComponent<BoxCollider2D>();
        colliderSensorMove = gameObject.transform.Find("EnemySensorMove").GetComponent<ColliderSensor>();
        colliderSensorAttack = gameObject.transform.Find("EnemySensorAttack").GetComponent<ColliderSensor>();

        anim = gameObject.transform.Find("Image").GetComponent<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        StatusInitialize();
    }

    protected virtual void OnEnable()
    {
        b_IsActive = true;

        StartCoroutine(AIalgorithm());
    }

    protected virtual void Update()
    {
        transform.position = new(transform.position.x, transform.position.y, transform.position.y / 100f);
    }

    IEnumerator AIalgorithm()
    {
        while (b_IsActive)
        {
            yield return null;

            if (colliderSensorMove.b_IsTarget)
            {
                if (colliderSensorAttack.b_IsTarget)
                {
                    anim.SetBool("b_Move", false);
                    anim.SetTrigger("t_Attack");

                    navMeshAgent.isStopped = true;

                    yield return new WaitForSeconds(1.1f);

                    playerPosition = colliderSensorAttack.v_targetPosition;
                    Attack();

                    yield return new WaitForSeconds(1.5f);
                }
                else
                {
                    anim.SetBool("b_Move", true);
                    playerPosition = colliderSensorMove.v_targetPosition;
                    Move();

                }
            }
            else if (Is_Order)
            {
                anim.SetBool("b_Move", true);
                playerPosition = orderPosition;
                Move();
            }
            else
            {
                anim.SetBool("b_Move", false);
                navMeshAgent.isStopped = true;
            }
        }
    }

    protected abstract void Attack();

    protected abstract void Move();

    public void Order(Vector2 pos)
    {
        Is_Order = true;
        orderPosition = pos;
    }

    protected void DieEndAnimation()
    {
        b_IsActive = false;
        gameObject.SetActive(false);
    }

    public abstract void Hit(IDamageble target);
    public abstract void Damaged(float life);
}
