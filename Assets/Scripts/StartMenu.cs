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
