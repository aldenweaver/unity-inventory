/* Code adapted from inScope Studios tutorial:
 * http://inscopestudios.com/Pages/Unity/Inventory.html
 * Typed and commented by Alden Weaver, Jan - Feb 2015
 */

using UnityEngine;
using System.Collections;

// Create the ItemTypes
public enum ItemType {HEALTH, DAMAGE};

public class Item : MonoBehaviour {

	// Returns the type of the item
	public ItemType type;

	// Item's sprites
	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;

	// Max number of times an item can stack on itself
	public int maxStackSize;


	public void Use() {
		switch (type) {
		case ItemType.HEALTH:
			Debug.Log ("Health potion used.");
			break;
		case ItemType.DAMAGE:
			Debug.Log ("Damage potion drank.");
			break;
		}
	}

	public void SetStats(Item item)
	{
		this.type = item.type;

		this.spriteNeutral = item.spriteNeutral;

		this.spriteHighlighted = item.spriteHighlighted;

		this.maxStackSize = item.maxStackSize;

		// Model for item
		//this.environItem = item.environItem;

		switch (type)
		{
		case ItemType.DAMAGE:
			GetComponent<Renderer>().material.color = Color.blue;
			break;
		case ItemType.HEALTH:
			GetComponent<Renderer>().material.color = Color.red;
			break;
		}
	}
}
