using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ItemScript : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerScript;
    [SerializeField] private int itemCost;
    [SerializeField] private string itemType;
    private string itemText;
    private GameObject itemObject;

    void Start()
    {
        itemObject = gameObject.transform.GetChild(0).gameObject;
        itemText = itemObject.GetComponent<TMPro.TextMeshPro>().text;

        switch (itemType)
        {
            case "Health":
                itemText = "15 coinses for an helfi posjen";
                break;
            case "Mana":
                itemText = "15 coinses for a mana posjen";
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(this.transform.position, 0.5f, 1 << 6))
        {
            playerScript.BuyItem(itemType, itemCost);
            itemObject.SetActive(true);
        }
        else
        {
            itemObject.SetActive(false);
        }
    }
}