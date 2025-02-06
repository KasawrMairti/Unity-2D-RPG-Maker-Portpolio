using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpirit : Monster
{
    protected override void StatusInitialize()
    {
        status.name =        "하급 불의 정령";
        status.description = "조금만한 불의 정령입니다.";
        status.lv =          1;
        status.hpMax =       4.0f;
        status.hp =          status.hpMax;
        status.attack =      1.0f;
        status.attackSpeed = 3.0f;
        status.deffence =    0.0f;
    }

    public override void Damaged(float life)
    {
        status.hp -= life - (status.deffence * 0.1f);

        if (status.hp <= 0)
        {
            DieEndAnimation();
        }
    }

    public override void Hit(IDamageble target)
    {
        if (target != null)
            target.Damaged(status.attack);
    }

    protected override void Attack()
    {        
        if (playerPosition.x < transform.position.x)
            transform.localScale = new Vector2(-1.0f, 1.0f);
        else
            transform.localScale = new Vector2(1.0f, 1.0f);

        bool newOBJ = true;
        foreach (var enemybullet in enemyBullets)
        {
            if (!enemybullet.b_IsActive)
            {
                enemybullet.gameObject.SetActive(true);
                enemybullet.transform.position = transform.position;
                enemybullet.Shot(transform.position, playerPosition, 500.0f);
                enemybullet.originMaster = this;

                newOBJ = false;
                break;
            }
        }
        if (newOBJ)
        {
            EnemyBullet enemybullet = Instantiate(enemyBulletOBJ).transform.GetComponent<EnemyBullet>();
            enemybullet.transform.position = transform.position;
            enemybullet.Shot(transform.position, playerPosition, 500.0f);
            enemybullet.originMaster = this;

            enemyBullets.Add(enemybullet);
        }
    }

    protected override void Move()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(new Vector3(playerPosition.x, playerPosition.y, transform.position.z));

        if (navMeshAgent.desiredVelocity.x < 0f)
            transform.localScale = new Vector2(-1.0f, 1.0f);
        else
            transform.localScale = new Vector2(1.0f, 1.0f);
    }

}
