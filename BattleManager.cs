using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager manager;
    private Queue<BattleEntity> turnOrder;
    private BattleEntity[] entities;
    [SerializeField]
    private Button fightButton;
    private Button fleeButton;

    // Start is called before the first frame update
    void Start()
    {
        if (manager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }

        entities = (BattleEntity[])FindObjectsOfType(typeof(BattleEntity));
        foreach (BattleEntity entity in entities)
        {
            turnOrder.Enqueue(entity);
        }
    }
    void PlayerMove(BattleEntity entity)
    {
        fightButton.interactable = true;
        fleeButton.interactable = true;
    }
    void EnemyMove()
    {

    }

    public void EnemyDied(string enemyName)
    {

    }

    public void PlayerDone()
    {

    }
}
