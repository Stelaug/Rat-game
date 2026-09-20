using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPipe : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform water_out_pipe;
    public Transform water_hider;
    public float timer = 6;
    public GameObject valveInformation;
    public bool pipe_opened;

    public GameObject rise1;
    public GameObject rise2;
    public GameObject rise3;
    public GameObject rise4;

    private void Start()
    {
        timer = PlayerPrefs.GetInt("timer");
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("pipe_opened") == 1)
        {
            if (PlayerPrefs.GetInt("pipe_opened") == 1)
            {
                timer = -9;
            }
            
            Waterfall();
        }
    }

    void Waterfall()
    {
        if (timer < -10)
        {
            water_out_pipe.Translate(20 * Vector3.down, Space.World);
            water_hider.Translate(20 * Vector3.up, Space.World);
            rise1.GetComponent<Collider2D>().enabled = true;
            rise2.GetComponent<Collider2D>().enabled = true;
            rise3.GetComponent<Collider2D>().enabled = true;
            rise4.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            water_out_pipe.Translate(2 * Vector3.down * Time.deltaTime, Space.World);
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                water_hider.Translate(Vector3.up * Time.deltaTime, Space.World);
                if (timer < -1)
                {
                    Debug.Log("hi");
                    rise1.GetComponent<Collider2D>().enabled = true;
                    if (timer < -2)
                    {
                        rise2.GetComponent<Collider2D>().enabled = true;
                        if (timer < -3)
                        {
                            rise3.GetComponent<Collider2D>().enabled = true;
                            if (timer < -4)
                            {
                                rise4.GetComponent<Collider2D>().enabled = true;
                            }
                        }
                    }

                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        valveInformation.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            PlayerPrefs.SetInt("pipe_opened", 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        valveInformation.SetActive(false);
    }

    private void OnDestroy()
    {
        int timerWater = (int)timer;
        PlayerPrefs.SetInt("timer", timerWater);
    }
}
