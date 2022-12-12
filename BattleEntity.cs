using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Damage(float percentage);
public class BattleEntity : MonoBehaviour
{
    public float maxHealth = 5f;
    public float curHealth = 5f;
    public float attack = 1f;
    public bool isPlayer = false;
    public bool dodging = false;
    [SerializeField]
    private Animator animator;
    public Damage damageEvent;

    public void Attack(BattleEntity victim)
    {
        dodging = false;
        if(isPlayer)
        {
            BattleManager.manager.prevPlayerAttack = true;
        }
        else
        {
            BattleManager.manager.prevEnemyAttack = true;
        }
        StartCoroutine(EndTurn());
        animator.SetTrigger("Attack");
        if (!victim.dodging)
        {
            victim.TakeDamage(attack);
        }
    }

    public void Dodge()
    {
        dodging = true;
        StartCoroutine(EndTurn());
        if (isPlayer)
        {
            BattleManager.manager.prevPlayerAttack = false;
        }
        else
        {
            BattleManager.manager.prevEnemyAttack = false;
        }
    }

    public void TakeDamage(float amount)
    {
        curHealth -= amount;
        if(curHealth <= 0)
        {
            curHealth = 0;
            Die();
        }
        damageEvent.Invoke(curHealth / maxHealth);
    }

    void Die()
    {
        BattleManager.manager.EnemyDied(gameObject.name);
        Destroy(gameObject);
    }

    public void TakeEnemyTurn(BattleEntity victim)
    {
        bool decision = GetComponent<DecisionTree>().ShouldAttack(curHealth / maxHealth, BattleManager.manager.prevEnemyAttack, BattleManager.manager.prevPlayerAttack);
        if(decision)
        {
            Debug.Log("Enemy attacks " + victim.gameObject.name);
            Attack(victim);
        }
        else
        {
            Debug.Log("Enemy Dodges");
            Dodge();
        }
    }

    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(1.5f);
        BattleManager.manager.TurnEnd();
    }
}
