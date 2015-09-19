using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevationTile : MonoBehaviour
{
	public GameObject[] tiles;
	public GameObject xMarksTheSpot;
	public GameObject hole;
	public GameObject ropeLadder;
	public GameObject chest;
	public GameObject tent;
	public GameObject randomItem;

	public void SmoothElevation(List<Tile> tiles) {

		Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;
		int height = tileMap.GetLength(0);
		int width = tileMap.GetLength(1);

		bool smoothing = true;
		while(smoothing) {
			smoothing = false;
			foreach (Tile tile in tiles) {

				if (tile.x > 0 && tile.y > 0 && tile.x < width - 1 && tile.y < height - 1) {
					for (int xDelta = -1; xDelta <= 1; xDelta++) {
						for (int yDelta = -1; yDelta <= 1; yDelta++) {
							if (tileMap[tile.x + xDelta, tile.y + yDelta].elevation + 1 < tile.elevation) {
								tile.elevation--;
								smoothing = true;
							}
						}
					}
				}
			}
		}
	}

	public void placeCliffTiles(List<Tile> tiles) {

		Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;
		int height = tileMap.GetLength(0);
		int width = tileMap.GetLength(1);

		foreach (Tile tile in tiles) {
			int x = tile.x;
			int y = tile.y;

			bool lower = false;
			List<int> walls = new List<int>();
			for (int xDelta = -1; xDelta <= 1; xDelta++) {
				for (int yDelta = -1; yDelta <= 1; yDelta++) {

					if (x + xDelta >= 0 && y + yDelta >= 0 && x + xDelta < width && y + yDelta < height &&
							tileMap[x + xDelta, y + yDelta].elevation > tileMap[x, y].elevation) {
						lower = true;
						walls.Add(xDelta + 1 + (yDelta + 1) * 3);
					}
				}
			}

			if (lower) {
				if (tile.path) {
					this.GetComponent<RoomManager>().SetGroundTile(this.tiles[0], x, y);
				} else {
					this.GetComponent<RoomManager>().PlaceItem(this.GetWallTile(walls), x, y);

					// Plains needs a special ground tile for layering
					if (tile.biome == this.GetComponent<PlainsTile>().getBiomeNumber()) {
						GameObject flatSprite = this.GetComponent<PlainsTile>().getFlatGroundTile();
						this.GetComponent<RoomManager>().SetGroundTile(flatSprite, x, y);
					}
				}
				tile.blocking = true;
			}
		}
	}

	public GameObject GetTreasureTile() {
		return this.xMarksTheSpot;
	}

	// DONT TOUCH MY MAGIC FUNCTION --Chris
	private GameObject GenerateWallTile(List<int> walls) {

		GameObject wallObject = new GameObject();

		if (walls.Contains(1)) {
			GameObject copy = Instantiate (this.tiles[1],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[1].transform.SetParent( wallObject.transform );
		}
		if (walls.Contains(7)) {
			GameObject copy = Instantiate (this.tiles[5],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//copy.transform.position = new Vector3(0f, 0f, 0f);
			//this.tiles[5].transform.SetParent( wallObject.transform );
		}
		if (walls.Contains(3)) {
			GameObject copy = Instantiate (this.tiles[3],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[3].transform.SetParent( wallObject.transform );
		}
		if (walls.Contains(5)) {
			GameObject copy = Instantiate (this.tiles[7],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[7].transform.SetParent( wallObject.transform );
		}

		if (walls.Contains(8) && !walls.Contains(7) && !walls.Contains(5)) {
			GameObject copy = Instantiate (this.tiles[6],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[6].transform.SetParent( wallObject.transform );
		}
		if (walls.Contains(6) && !walls.Contains(7) && !walls.Contains(3)) {
			GameObject copy = Instantiate (this.tiles[4],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[4].transform.SetParent( wallObject.transform );
		}
		if (walls.Contains(2) && !walls.Contains(1) && !walls.Contains(5)) {
			GameObject copy = Instantiate (this.tiles[8],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[8].transform.SetParent( wallObject.transform );
		}
		if (walls.Contains(0) && !walls.Contains(1) && !walls.Contains(3)) {
			GameObject copy = Instantiate (this.tiles[2],
																		 new Vector3(0f, 0f, 0f),
																		 Quaternion.identity) as GameObject;
			copy.transform.SetParent( wallObject.transform );
			//this.tiles[2].transform.SetParent( wallObject.transform );
		}
		/*if (walls.Contains(2)) {
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
		return this.tiles[0];*/

		return wallObject;
	}

	public static Dictionary<string, GameObject> wallsHash = new Dictionary<string, GameObject>();
	private GameObject GetWallTile(List<int> walls) {

		string key = "";
		foreach (int number in walls) {
			key += number;
		}

		GameObject newWall = null;
		if (!ElevationTile.wallsHash.TryGetValue(key, out newWall)) {
			newWall = this.GenerateWallTile(walls);
			ElevationTile.wallsHash.Add(key, newWall);
		}
		return newWall;
	}

	public static void ClearWallsHash() {
		foreach (GameObject wall in ElevationTile.wallsHash.Values) {
			Destroy (wall);
		}
		ElevationTile.wallsHash = new Dictionary<string, GameObject>();
	}

	public GameObject GetXMarksTheSpot() {
		return this.xMarksTheSpot;
	}

	public GameObject GetHole() {
		return this.hole;
	}
}

