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
    [SerializeField]
    private Animator animator;
    public Damage damageEvent;

    public void Attack(BattleEntity victim)
    {
        animator.SetTrigger("Attack");
        victim.TakeDamage(attack);
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
}
