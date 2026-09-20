using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage_popup : MonoBehaviour
{
    [SerializeField] private GameObject txt; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void display_damage(int damage, Vector3 pos, Color colour){
        GameObject text = Instantiate(txt, new Vector3(pos.x+Random.Range(-0.2f, 0.2f), pos.y+Random.Range(-0.2f, 0.2f), pos.z), Quaternion.identity) as GameObject;
        text.GetComponent<TMPro.TextMeshPro>().text = damage.ToString();
        text.GetComponent<TMPro.TextMeshPro>().faceColor = colour;
        StartCoroutine(Delay(text));



    }
    private IEnumerator Delay(GameObject popup){
        yield return new WaitForSeconds(1f);
        Destroy(popup);
    }
}
