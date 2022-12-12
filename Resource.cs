using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Player player;
    public float timeToMine;
    public float skillType;
    public float exp;
    
    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnMouseDown()
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        if (player.GetSkillLevel(skillType) != 0)
        {
            yield return new WaitForSeconds((timeToMine / 4) + (timeToMine * 0.75f) / player.GetSkillLevel(skillType));
            player.ChangeSkillLevel(skillType, exp);
        }
    }
}
