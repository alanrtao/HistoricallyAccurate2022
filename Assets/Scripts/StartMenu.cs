using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    int story_index = 0;

    public GameObject[] texts;

    public void ReadStory()
    {
        if (story_index == texts.Length - 1)
        {
            StartGame();
            return;
        }
        texts[story_index].gameObject.SetActive(false);
        texts[story_index + 1].gameObject.SetActive(true);
        story_index++;
    }

    public void WinScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
    
    public void LoseScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void StartScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
