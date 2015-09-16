using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class StartTile : MonoBehaviour {

  public GameObject deadBandit;
  public GameObject deadBoy;
  public GameObject deadGirl;
  public GameObject startingWeapon;
  public GameObject[] startingTools;

  public int range = 7;

  public void PlaceStartTiles() {
    Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;

    for (var i = 0; i < 3; i++) {
      this.PlaceInStartingRange(deadBandit);
    }

    this.PlaceInStartingRange(deadGirl);
    this.PlaceInStartingRange(deadBoy);
    this.PlaceInStartingRange(startingWeapon);
    this.PlaceInStartingRange(startingTools[Random.Range(0, startingTools.Length)]);
  }

  private void PlaceInStartingRange(GameObject sprite) {
    Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;

    int x = 16 + Random.Range((int) (-range / 2), (int) (range / 2));
    int y = 16 + Random.Range((int) (-range / 2), (int) (range / 2));
    while (tileMap[x, y].blocking || tileMap[x, y].path) {
      x = 16 + Random.Range((int) (-range / 2), (int) (range / 2));
      y = 16 + Random.Range((int) (-range / 2), (int) (range / 2));
    }

    this.GetComponent<RoomManager>().PlaceItem(sprite, x, y);

    // Flip tile half of the time
    if (Random.Range(0, 2) == 1) {
      tileMap[x, y].item.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
  }
}
