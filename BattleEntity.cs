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

    // Attack function, updates battle state
    public void Attack(BattleEntity victim)
    {

        BattleManager.manager.SetBattleText("");
        dodging = false;
        if(isPlayer)
        {
            BattleManager.manager.battleText.text += "Player Attacks! ";
            BattleManager.manager.prevPlayerAttack = true;
        }
        else
        {
            BattleManager.manager.battleText.text += "Enemy Attacks! ";
            BattleManager.manager.prevEnemyAttack = true;
        }
        EndTurn();
        animator.SetTrigger("Attack");
        if (!victim.dodging)
        {
            victim.TakeDamage(attack);
        }
        else
        {
            BattleManager.manager.battleText.text += "But it was dodged! ";
            victim.DodgeAnimation();
        }
    }

    // Dodge function, updates battle state
    public void Dodge()
    {
        dodging = true;
        EndTurn();
        EndTurn();
        if (isPlayer)
        {
            BattleManager.manager.prevPlayerAttack = false;
        }
        else
        {
            BattleManager.manager.prevEnemyAttack = false;
        }
        if(!(BattleManager.manager.prevEnemyAttack || BattleManager.manager.prevPlayerAttack))
        {
            dodging = false;
        }
    }

    // Dodging animation trigger, called by Attacker entity
    public void DodgeAnimation()
    {
        animator.SetTrigger("Dodge");
    }

    // Taking damage, called by Attacker entity
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

    // Death function
    void Die()
    {
        BattleManager.manager.EnemyDied(gameObject.name);
        Destroy(gameObject);
    }

    // Enemy AI function
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

    // Coroutine to end the current turn after some time
    private void EndTurn()
    {
        StartCoroutine(EndTurnWithDelay());
    }

    private IEnumerator EndTurnWithDelay(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        BattleManager.manager.NextMove();
    }
}
