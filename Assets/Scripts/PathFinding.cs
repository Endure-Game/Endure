using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {

	public GameObject world;
	private RoomManager roomManager;
	
	public void start (){
		this.roomManager = this.world.GetComponent<RoomManager> ();
	}

	public List<Vector3> PathFinder (Vector3 current, Vector3 destination) {

		List<Vector3> finalPath = new List<Vector3> ();

		while (current.x != destination.x && current.y != destination.y) {
			List<Vector3> posPaths = this.OpenTiles (current);
			Vector3 temp = new Vector3(100000f, 100000f, 100000f);

			for(int i = 0; i < posPaths.Count - 1; i++){
				posPaths[i] = distance(posPaths[i], destination);
				if(temp == null){
					temp = posPaths[i];
				}else if(posPaths[i].z < temp.z){
					temp = posPaths[i];
				}
			}
			finalPath.Add(temp);
			current = temp;

		}
		return finalPath;
	}

	private Vector3 distance (Vector3 position, Vector3 endPosition){
		float x = position.x;
		float y = position.y;
		float endX = endPosition.x;
		float endY = endPosition.y;

		float distance = Mathf.Sqrt (Mathf.Pow(Mathf.Abs(x - endX),2) + Mathf.Pow(Mathf.Abs(y - endY),2));

		position.z = distance;

		return position;
	}

	private List<Vector3> OpenTiles (Vector3 currentPos) {

		float x = currentPos.x;
		float y = currentPos.y;

		Vector3 north = new Vector3(x, y + 1f, 0f);
		Vector3 northEast = new Vector3(x + 1f, y + 1f, 0f);
		Vector3 east = new Vector3(x + 1f, y, 0f);
		Vector3 southEast = new Vector3(x + 1f, y - 1f, 0f);
		Vector3 south = new Vector3(x, y - 1f, 0f);
		Vector3 southWest = new Vector3(x - 1f, y - 1f, 0f);
		Vector3 west = new Vector3(x - 1f, y, 0f);
		Vector3 northWest = new Vector3(x - 1f, y + 1f, 0f);

		Vector3[] possibleMoves = new Vector3[8] {north, northEast, east, southEast, south, southWest, west, northWest};
		List<Vector3> nonBlockedTiles = new List<Vector3>();

		for (int i = 0; i < possibleMoves.Length - 1; i++) {
			if(roomManager.checkForBlock(possibleMoves[i]) != false){
				if(possibleMoves[i].z == 0){
					nonBlockedTiles.Add(possibleMoves[i]);
				}
			}
		}
		return nonBlockedTiles;

	}
	

}
