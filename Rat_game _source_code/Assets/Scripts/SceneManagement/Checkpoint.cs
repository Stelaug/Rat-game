using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    public int num;
    void OnTriggerEnter2D(Collider2D other) {
        PlayerPrefs.SetInt("checkpoint", num);
        PlayerPrefs.SetInt("currHealth", 100);
        PlayerPrefs.SetInt("scene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();   
    }
}
