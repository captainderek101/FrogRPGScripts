using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float reputation = 0;
    public float repExp = 0;
    public float repExpToLVL = 10;

    public float gathering = 1;
    public float gatExp = 0;
    public float gatExpToLVL = 25;

    public float combat = 1;
    public float cbtExp = 0;
    public float cbtExpToLVL = 25;

    public float communication = 1;
    public float comExp = 0;
    public float comExpToLVL = 25;
    public void ChangeSkillLevel(float skillType, float exp) // 0 = reputation, 1 = gathering, 2 = combat, 3 = communication
    {
        if(skillType == 0)
        {
            repExp += exp;
            while(repExp > repExpToLVL)
            {
                reputation++;
                repExp -= repExpToLVL;
                repExpToLVL += 5;
            }
        }
        else  if (skillType == 1)
        {
            gatExp += exp;
            while (gatExp > gatExpToLVL)
            {
                gathering++;
                gatExp -= gatExpToLVL;
                gatExpToLVL += 10;
            }
        }
        else if (skillType == 2)
        {
            cbtExp += exp;
            while (cbtExp > cbtExpToLVL)
            {
                combat++;
                cbtExp -= cbtExpToLVL;
                cbtExpToLVL += 10;
            }
        }
        else if (skillType == 3)
        {
            comExp += exp;
            while (comExp > comExpToLVL)
            {
                communication++;
                comExp -= comExpToLVL;
                comExpToLVL += 10;
            }
        }
        else
        {
            return;
        }
    }
    public float GetSkillLevel(float skillType)
    {
        if (skillType == 0)
        {
            return reputation;
        }
        else if (skillType == 1)
        {
            return gathering;
        }
        else if (skillType == 2)
        {
            return combat;
        }
        else if (skillType == 3)
        {
            return communication;
        }
        else
        {
            return 0;
        }
    }
}
