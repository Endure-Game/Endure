using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drops : MonoBehaviour {

  [System.Serializable]
  public class Drop {
    public float chance;
    public GameObject item;
  }

  public Drop[] drops;

  // Need to make sure that we don't instantiate objects when game ends
  // or they will be left in the object heirarchy

  public void DropItem() {

    // Get only items that are not in player inventory
    float max = 0f;
    List<Drop> newDrops = new List<Drop>();
    foreach (Drop drop in this.drops) {

      bool found = false;
      foreach (PlayerController.InventoryItem item in PlayerController.instance.inventory) {
        if (item.name == drop.item.name) {
          found = true;
        }
      }

      if (!found) {
        max += drop.chance;
        newDrops.Add(drop);
      }
    }
    if (max < 1f) {
      max = 1f;
    }

    float current = 0f;
    float selected = Random.Range (0f, max);
    foreach (var drop in newDrops) {
      current += drop.chance;
      if (current >= selected) {
        Instantiate (drop.item, this.transform.position, Quaternion.identity);
        break;
      }
    }
  }
}
