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
    private AudioSource transitionMusic;
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
        StartCoroutine(LevelTransition(transitiionTime));
    }

    IEnumerator LevelTransition(float seconds)
    {
        transitionMusic.Play();
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Battle");
    }
}
