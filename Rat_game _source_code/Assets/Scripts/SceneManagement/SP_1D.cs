using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_1D : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform sp_1;
    [SerializeField] private Transform sp_2;
    [SerializeField] private Transform sp_3;
    [SerializeField] private Transform sp_4;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("SP_1D") == 0){
            PlayerPrefs.SetInt("SP_1D", 1);
            PlayerPrefs.Save();
        }
        if(PlayerPrefs.GetInt("SP_1D") == 1){
            player.position = sp_1.position;
            }
    else if(PlayerPrefs.GetInt("SP_1D") == 2){
            player.position = sp_2.position;
            }
    else if(PlayerPrefs.GetInt("SP_1D") == 3){
            player.position = sp_3.position;
            }
    else if(PlayerPrefs.GetInt("SP_1D") == 4){
            player.position = sp_4.position;
            }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}