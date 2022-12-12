using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager manager;
    private Queue<BattleEntity> turnOrder = new Queue<BattleEntity>();
    private BattleEntity[] entities;
    [SerializeField]
    private Button fightButton;
    [SerializeField]
    private Button fleeButton;
    public bool prevEnemyAttack = false;
    public bool prevPlayerAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (manager != null)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        manager = this;
        //    DontDestroyOnLoad(gameObject);
        //}

        entities = Object.FindObjectsOfType(typeof(BattleEntity)) as BattleEntity[];
        foreach (BattleEntity entity in entities)
        {
            Debug.Log("Adding Entity "+entity.gameObject.name);
            turnOrder.Enqueue(entity);
        }
        if(turnOrder.Peek().isPlayer)
        {
            PlayerMove(turnOrder.Peek());
        }
        else
        {
            EnemyMove(turnOrder.Peek());
        }
    }

    // Call to start Player Turn
    void PlayerMove(BattleEntity player)
    {
        Debug.Log("Player Turn.");
        turnOrder.Dequeue();
        turnOrder.Enqueue(player);
        fightButton.interactable = true;
        fleeButton.interactable = true;
    }

    // Called by Buttons to turn them off once turn was made
    public void DisableButtons()
    {
        fightButton.interactable = false;
        fleeButton.interactable = false;
    }

    // Call to start Enemy Turn
    void EnemyMove(BattleEntity enemy)
    {
        Debug.Log("Enemy Turn.");
        turnOrder.Dequeue();
        turnOrder.Enqueue(enemy);
        enemy.TakeEnemyTurn(turnOrder.Peek());
    }

    // Called by entity when it dies
    public void EnemyDied(string enemyName)
    {

    }

    // Called by entity after some delay
    public void TurnEnd()
    {
        if (turnOrder.Peek().isPlayer)
        {
            PlayerMove(turnOrder.Peek());
        }
        else
        {
            EnemyMove(turnOrder.Peek());
        }
    }
}
