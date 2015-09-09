using UnityEngine;
using System.Collections;

public class Drops : MonoBehaviour {

  [System.Serializable]
  public class Drop {
    public float chance;
    public GameObject item;
  }

  private bool isQuitting = false;

  public Drop[] drops;

  // Need to make sure that we don't instantiate objects when game ends
  // or they will be left in the object heirarchy
  void OnDisable() {
    isQuitting = true;
  }

  void OnDestroy() {

    if (isQuitting) {
      return;
    }

    float max = 0f;
    foreach (var drop in this.drops) {
      max += drop.chance;
    }
    if (max < 1f) {
      max = 1f;
    }

    float current = 0f;
    float selected = Random.Range (0f, max);
    foreach (var drop in this.drops) {
      current += drop.chance;
      if (current >= selected) {
        Instantiate (drop.item, this.transform.position, Quaternion.identity);
        break;
      }
    }
  }
}
