using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class location_manager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform sp_1;
    [SerializeField] private Transform sp_2;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("spawnpoint") == 0){
            PlayerPrefs.SetInt("spawnpoint", 1);
            PlayerPrefs.Save();
        }
        if(PlayerPrefs.GetInt("spawnpoint") == 1){
            player.position = sp_1.position;
            }
    else if(PlayerPrefs.GetInt("spawnpoint") == 2){
            player.position = sp_2.position;
            }

    }

    // Update is called once per frame
    void Update()
    {

        // temp
        if(Input.GetKeyDown(KeyCode.Backspace)){
            if(PlayerPrefs.GetInt("spawnpoint") == 1){
            PlayerPrefs.SetInt("spawnpoint", 2);
            PlayerPrefs.Save();
            }
            else{
                PlayerPrefs.SetInt("spawnpoint", 1);
                PlayerPrefs.Save();
            }
        }
        if(Input.GetKeyDown(KeyCode.P)){
            if(PlayerPrefs.GetInt("spawnpoint") == 1){
                player.position = sp_1.position;
            }
            else if(PlayerPrefs.GetInt("spawnpoint") == 2){
                player.position = sp_2.position;
            }
        }
        // perm
        
    }
}
