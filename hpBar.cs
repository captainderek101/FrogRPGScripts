using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private BattleEntity entity;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        entity.damageEvent += UpdateUI;
    }

    void UpdateUI(float percentage)
    {
        slider.value = percentage;
    }
}
