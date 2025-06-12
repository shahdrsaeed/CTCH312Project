using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    private InventoryManager inventoryManager;
    int numGems = 0;
    AudioSource thisAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider trigger) 
    {
        if (trigger.tag == "Gem")
        {
            inventoryManager.AddItem(itemName, quantity, sprite);
            trigger.gameObject.SetActive(false);
            numGems = numGems + quantity;
            thisAudio.Play();
            Destroy(gameObject);

        }
    }
}
