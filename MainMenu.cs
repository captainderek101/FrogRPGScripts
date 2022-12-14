using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;

    [SerializeField] private GameObject player;

    private void Start()
    {
        Destroy(GameObject.Find("Player"));
    }

    public void Play()
    {
        Instantiate(player);
        menuUI.SetActive(false);
    }

    private void OnLevelWasLoaded(int level)
    {
        menuUI.SetActive(false);
    }
}
