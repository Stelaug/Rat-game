using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float distanceFromCamera;
    private float moveSpeed;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    [SerializeField] private Transform player;
    private Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = new Vector2((player.position.x * -distanceFromCamera) + xOffset, (player.position.y * -distanceFromCamera) + yOffset);
    }
}
