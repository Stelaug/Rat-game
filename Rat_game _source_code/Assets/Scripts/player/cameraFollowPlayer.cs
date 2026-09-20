using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
 public class cameraFollowPlayer : MonoBehaviour {
    // Variable declarations
     public float dampTime = 0.0f;
     private Vector3 velocity = Vector3.zero;
     public Transform target;
     void Start(){
        StartCoroutine(teleport());
     }
     void Update () 
     {
        // Smoothly moves the camera towards the player
        Vector3 destination = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
         transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
     }
     private IEnumerator teleport(){
        yield return new WaitForSeconds(0.001f);
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
 }
 }