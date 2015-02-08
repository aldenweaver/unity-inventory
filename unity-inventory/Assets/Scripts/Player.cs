using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;

	// The reference to the inventory
	public Inventory inventory;


	// Use this for initialization
	void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
		HandleMovement();
	}


	// Handles the player's movement
	private void HandleMovement() {
		// Make it frame rate independent in order to ensure that
		// the player moves at same frame rate no matter the device
		float translation = speed * Time.deltaTime;

		transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, 0, Input.GetAxis("Vertical") * translation));
	}


	// Called when player collides with something 
	private void OnTriggerEnter(Collider other) {
		// If the object is collectable, add it to the inventory via its Item component
		if (other.tag == "Item") {
			inventory.AddItem(other.GetComponent<Item>());
		}
	}
		                            
}
