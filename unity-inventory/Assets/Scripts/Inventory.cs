using UnityEngine;
using System.Collections;

// Include lists
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	// Use for changing inventory's appearance
	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;

	// Use for setting inventory layout from Inspector
	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;

	// Set size of slots
	public float slotSize;

	// Grab Slot prefab to instantiate when needed
	public GameObject slotPrefab;

	// Holds each slot in the inventory
	private List<GameObject> allSlots;

	// Keeps track of how many empty slots there are in the inventory
	private int emptySlots;


	// Use this for initialization
	void Start () {
		CreateLayout();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Set up inventory and slots
	private void CreateLayout() {
		// Instantiate allSlots & emptySlots
		allSlots = new List<GameObject>();
		emptySlots = slots;

		// Calculate width and height of inventory
		float inventoryWidth = ((slots/rows)*(slotSize + slotPaddingLeft)) + slotPaddingLeft;
		float inventoryHeight = (rows * (slotSize + slotPaddingTop)) + slotPaddingTop;

		// Get inventory's rect transform and set width and height
		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		// Calculate # of columns
		int columns = slots / rows;

		// Place slots on inventory
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				// Create a new slot and access it's RectTransform
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				// Set properties for the slot
				newSlot.name = "Slot";
				newSlot.transform.SetParent(this.transform.parent);

				// Set the position of the slot within the inventory
				slotRect.localPosition = inventoryRect.localPosition + (new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y)));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);


				// Add newSlot to allSlots list
				allSlots.Add(newSlot);

//				float xPosition = slotPaddingLeft * (x + 1) + (slotSize * x);
//				float yPosition = slotPaddingTop * (y + 1) + (slotSize * y);
			}
		}
	}


	//
	public bool AddItem(Item item) {
		// if the item is not stackable...
		if (item.maxStackSize == 1) {
			PlaceEmpty(item);
			return true;
		}
		return false;
	}


	//
	private bool PlaceEmpty(Item item) {
		if (emptySlots > 0) {
			foreach(GameObject slot in allSlots) {
				Slot temp = slot.GetComponent<Slot>();

				if(temp.IsEmpty) {
					temp.AddItem(item);
					emptySlots--;
					return true;
				}
			}
		}
		Debug.Log("No more empty slots");
		return false;
	}
}
