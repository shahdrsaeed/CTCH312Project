using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Texture2D spriteTexture;

	private Sprite sprite;

    private InventoryManager inventoryManager;
    int numGems = 0;
    AudioSource thisAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisAudio = GetComponent<AudioSource>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    
	sprite = Sprite.Create(spriteTexture, new Rect(0.0f, 0.0f, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f), 100.0f);


    }

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
	    inventoryManager.AddItem(itemName, quantity, sprite);
            //thisAudio.Play();
		collision.GetComponent<AudioSource>().Play();
            this.gameObject.SetActive(false);
        }
    }
}
