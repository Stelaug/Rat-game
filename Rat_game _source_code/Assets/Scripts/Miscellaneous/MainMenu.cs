using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButtonDown("Cancel") && SceneManager.GetSceneByBuildIndex(8).isLoaded)
        {
            QuitGame();
        }
    }
    public void LoadScene(int levelIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerPrefs.GetInt("scene"));
    }
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
