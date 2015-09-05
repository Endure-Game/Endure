using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevationTile : MonoBehaviour
{
	public GameObject[] tiles;
	
	public void placeCliffTiles() {

		Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;
		int height = tileMap.GetLength(0);
		int width = tileMap.GetLength(1);

		for (int x = 1; x < width - 1; x++) {
			for (int y = 1; y < height - 1; y++) {

				bool lower = false;
				List<int> walls = new List<int>();
				for (int xDelta = -1; xDelta <= 1; xDelta++) {
					for (int yDelta = -1; yDelta <= 1; yDelta++) {
						if (tileMap[x + xDelta, y + yDelta].elevation > tileMap[x, y].elevation) {
							lower = true;
							walls.Add(xDelta + 1 + (yDelta + 1) * 3);
						}
					}
				}

				if (lower) {
					if (tileMap[x, y].path) {
						this.GetComponent<RoomManager>().SetGroundTile(tiles[0], x, y);
					} else {
						this.GetComponent<RoomManager>().PlaceItem(GetWallTile(walls), x, y);
					}
				}
			}
		}
	}

	// DONT TOUCH MY MAGIC FUNCTION --Chris
	private GameObject GetWallTile(List<int> walls) {

		if (walls.Contains(2)) {
			if (walls.Contains(1) && walls.Contains(5)) {
				return this.tiles[10];
			} else if (walls.Contains(1)) {
				if (walls.Contains(3)) {
					return this.tiles[11];
				} else {
					return this.tiles[1];
				}
			} else if (walls.Contains(5)) {
				if (walls.Contains(7)) {
					return this.tiles[9];
				} else {
					return this.tiles[7];
				}
			} else {
				return this.tiles[8];
			}
		} else if (walls.Contains(0)) {
			if (walls.Contains(1) && walls.Contains(3)) {
				return this.tiles[11];
			} else if (walls.Contains(1)) {
				return this.tiles[1];
			} else if (walls.Contains(3)) {
				if (walls.Contains(7)) {
					return this.tiles[12];
				} else {
					return this.tiles[3];
				}
			} else {
				return this.tiles[2];
			}
		} else if (walls.Contains(6)) {
			if (walls.Contains(7) && walls.Contains(3)) {
				return this.tiles[12];
			} else if (walls.Contains(7)) {
				return this.tiles[5];
			} else if (walls.Contains(3)) {
				return this.tiles[3];
			} else {
				return this.tiles[4];
			}
		} else if (walls.Contains(8)) {
			if (walls.Contains(7) && walls.Contains(5)) {
				return this.tiles[9];
			} else if (walls.Contains(7)) {
				return this.tiles[5];
			} else if (walls.Contains(5)) {
				return this.tiles[7];
			} else {
				return this.tiles[6];
			}
		}

		if (walls.IndexOf(1) != -1) {
			return this.tiles[1];
		} else if (walls.IndexOf(3) != -1) {
			return this.tiles[3];
		} else if (walls.IndexOf(5) != -1) {
			return this.tiles[7];
		} else if (walls.IndexOf(7) != -1) {
			return this.tiles[5];
		}
		print ("elevation placement probems --Show Chris your game if you see this");
		return this.tiles[0];
	}
}

