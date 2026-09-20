using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_playerprefs : MonoBehaviour
{
    public string room;
    public int SP;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerPrefs.SetInt(room, SP);
        PlayerPrefs.Save();
    }
}
