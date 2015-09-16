using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class RandomItem : MonoBehaviour {

  public GameObject[] weapons;
  public GameObject[] tools;
  public GameObject[] upgrades;

	// Use this for initialization
	void Start () {

    // Combine all items into one group of items
    GameObject[] allItems = new GameObject[weapons.Length + tools.Length + upgrades.Length];
    weapons.CopyTo(allItems, 0);
    tools.CopyTo(allItems, weapons.Length);
    upgrades.CopyTo(allItems, weapons.Length + tools.Length);

    // Pick one item at random and spawn it
    GameObject randomItem = allItems[Random.Range(0, allItems.Length)];
    GameObject newItem = Instantiate(randomItem,
                                     this.transform.position,
                                     Quaternion.identity) as GameObject;
    newItem.transform.parent = this.transform;
	}

	// Update is called once per frame
	void Update () {

	}
}
