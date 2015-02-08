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
}
