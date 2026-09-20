using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private void Start() {
        PlayerPrefs.SetInt("checkpoint", 0);
        PlayerPrefs.Save();
    }
}
