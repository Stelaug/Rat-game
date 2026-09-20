using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_1B : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform sp_1;
    [SerializeField] private Transform sp_2;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("SP_1B") == 0){
            PlayerPrefs.SetInt("SP_1B", 1);
            PlayerPrefs.Save();
        }
        if(PlayerPrefs.GetInt("SP_1B") == 1){
            player.position = sp_1.position;
            }
    else if(PlayerPrefs.GetInt("SP_1B") == 2){
            player.position = sp_2.position;
            }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
