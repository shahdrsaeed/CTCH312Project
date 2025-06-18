using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

	int numGems = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         InventoryMenu.SetActive(false);
         menuActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated) 
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
	numGems += quantity;
        //print("itemName = " + itemName + ", quantity = " + numGems + ", itemSprite = " + itemSprite);
    }
}
