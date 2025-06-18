using UnityEngine;

public class buttonUI : MonoBehaviour
{

    public GameObject InventoryDescription;
    public GameObject MapDescription;
    public GameObject GearDescription;
    //public int descriptionActivated; // 1 = inventory, 2 = map, 3 = gear

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryDescription.SetActive(true);
        //descriptionActivated = 1;
        MapDescription.SetActive(false);
        GearDescription.SetActive(false);
    }

    // Update is called once per frame
    public void newGameButton()
    {
        if (InventoryDescription || GearDescription) {
            InventoryDescription.SetActive(false);
            GearDescription.SetActive(false);
            MapDescription.SetActive(true);
        }
    }
}
