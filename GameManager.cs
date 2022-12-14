using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    [SerializeField]
    private float transitiionTime = 1.4f;
    [SerializeField]
    private AudioSource worldMusic;
    [SerializeField]
    private AudioSource transitionMusic;
    public bool LeonardDead = false;
    public bool CarltonDead = false;
    // Start is called before the first frame update
    void Start()
    {
        if(manager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartBattle(int enemyType)
    {
        StartCoroutine(LevelTransition(transitiionTime, enemyType));
    }

    IEnumerator LevelTransition(float seconds, int enemyType)
    {
        worldMusic.Pause();
        transitionMusic.Play();
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Battle "+enemyType);
    }

    public void GoToOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name == "Overworld")
            worldMusic.Play();
        if (CarltonDead)
        {
            Destroy(GameObject.Find("Carlton the Snake"));
        }
        if(LeonardDead)
        {
            Destroy(GameObject.Find("Leonard the Chameleon"));
        }
    }
}
