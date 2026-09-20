using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_menu : MonoBehaviour
{
    [SerializeField] private GameObject pause_menu;
    [SerializeField] private TMPro.TextMeshProUGUI MPD;
    [SerializeField] private TMPro.TextMeshProUGUI HPD;
    [SerializeField] private GameObject cover;
    private float t = 0.0f; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t > 0.1f){
            cover.SetActive(false);
        }
        MPD.text = PlayerPrefs.GetInt("MP_P").ToString();
        HPD.text = PlayerPrefs.GetInt("HP_P").ToString();
        if(Input.GetKeyDown(KeyCode.Escape) && pause_menu.activeSelf == false || Input.GetKeyDown(KeyCode.Joystick1Button7) && pause_menu.activeSelf == false){
            pause_menu.SetActive(true);
            Time.timeScale = 0;

        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pause_menu.activeSelf == true|| Input.GetKeyDown(KeyCode.Joystick1Button7) && pause_menu.activeSelf == true){
            pause_menu.SetActive(false);
            Time.timeScale = 1;
            

        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(8);
    }
    public void UnPause(){
        pause_menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Quit(){
        Application.Quit();
        print("quitting");
    }
}
