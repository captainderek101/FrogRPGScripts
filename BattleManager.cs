using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager manager;
    private Queue<BattleEntity> turnOrder = new Queue<BattleEntity>();
    private BattleEntity[] entities;
    [SerializeField]
    private Button fightButton;
    [SerializeField]
    private Button fleeButton;
    public TMP_Text battleText;
    public bool prevEnemyAttack = false;
    public bool prevPlayerAttack = false;
    private int turnCount;

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

    public void NextMove()
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
        if(enemyName == "Leonard")
        {
            GameManager.manager.LeonardDead = true;
        }
        else if(enemyName == "Carlton")
        {
            GameManager.manager.CarltonDead = true;
        }
        GameManager.manager.GoToOverworld();
    }

    //// Called by entity after some delay
    //public void TurnEnd()
    //{
    //    turnCount++;
    //    if (turnCount % 2 == 1)
    //    {
    //        NextMove();
    //    }
    //    else
    //    {
    //        if(prevEnemyAttack && prevPlayerAttack)
    //        {

    //            StartCoroutine(PlayMoves(2f));
    //        }
    //        else if(prevEnemyAttack || prevPlayerAttack)
    //        {
    //            StartCoroutine(PlayMoves(1f));
    //        }
    //        else
    //        {
    //            SetBattleText("Both Dodged!");
    //            StartCoroutine(PlayMoves(0.5f));
    //        }
    //    }
    //}

    public void SetBattleText(string text)
    {
        battleText.text = text;
    }

    private IEnumerator PlayMoves(float seconds = 1.5f)
    {
        yield return new WaitForSeconds(seconds);
        NextMove();
    }
}
