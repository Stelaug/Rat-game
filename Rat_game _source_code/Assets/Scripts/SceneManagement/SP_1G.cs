using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_1G : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform sp_1;
    [SerializeField] private Transform sp_2;
    [SerializeField] private Transform sp_3;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("SP_1G") == 0){
            PlayerPrefs.SetInt("SP_1G", 1);
            PlayerPrefs.Save();
        }
        if(PlayerPrefs.GetInt("SP_1G") == 1){
            player.position = sp_1.position;
            }
        else if(PlayerPrefs.GetInt("SP_1G") == 2){
            player.position = sp_2.position;
            }
        else if(PlayerPrefs.GetInt("SP_1G") == 3){
            player.position = sp_3.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}