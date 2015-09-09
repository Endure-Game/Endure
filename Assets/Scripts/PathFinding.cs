using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {

	public GameObject world;
	public int rows;
	public int columns;
	private RoomManager roomManager;
	

	public void Start (){
		this.roomManager = this.world.GetComponent<RoomManager> ();
		this.rows = roomManager.rows;
		this.columns = roomManager.columns;
	}

	public List<Vector3> PathFinder (Vector3 current, Vector3 destination) {

		List<Vector3> finalPath = new List<Vector3> ();

		while (current.x != destination.x && current.y != destination.y) {
			List<Vector3> posPaths = this.OpenTiles (current, finalPath);
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

	private List<Vector3> OpenTiles (Vector3 currentPos, List<Vector3> path) {

		float x = currentPos.x;
		float y = currentPos.y;
		Vector3 north;
		Vector3 northEast;
		Vector3 east;
		Vector3 southEast;
		Vector3 south;
		Vector3 southWest;
		Vector3 west;
		Vector3 northWest;

		if (y + 1 < columns - 2) {
			north = new Vector3 (x, y + 1f, 0f);
		}else {
			north = new Vector3 (0f,0f,0f);
		}
		if (x + 1 < rows - 2 && y + 1 < columns - 2) {
			northEast = new Vector3 (x + 1f, y + 1f, 0f);
		} else {
			northEast = new Vector3 (0f,0f,0f);
		}
		if (x + 1 < rows - 2) {
			east = new Vector3 (x + 1f, y, 0f);
		}else {
			east = new Vector3 (0f,0f,0f);
		}
		if (x + 1 < rows - 2 && y - 1 < columns - 2) {
			southEast = new Vector3 (x + 1f, y - 1f, 0f);
		}else {
			southEast = new Vector3 (0f,0f,0f);
		}
		if (y - 1 < columns - 2) {
			south = new Vector3 (x, y - 1f, 0f);
		}else {
			south = new Vector3 (0f,0f,0f);
		}
		if (x - 1 < rows - 2 && y - 1 < columns - 2) {
			southWest = new Vector3 (x - 1f, y - 1f, 0f);
		}else {
			southWest = new Vector3 (0f,0f,0f);
		}
		if (x - 1 < rows - 2) {
			west = new Vector3 (x - 1f, y, 0f);
		}else {
			west = new Vector3 (0f,0f,0f);
		}
		if (x - 1 < rows - 2 && y + 1 < columns - 2) {
			northWest = new Vector3 (x - 1f, y + 1f, 0f);
		}else {
			northWest = new Vector3 (0f,0f,0f);
		}

		Vector3[] possibleMoves = new Vector3[8] {north, northEast, east, southEast, south, southWest, west, northWest};
		List<Vector3> nonBlockedTiles = new List<Vector3>();

		for (int i = 0; i < possibleMoves.Length - 1; i++) {
			if(this.roomManager.checkForBlock(possibleMoves[i]) != false){
				float tempX = possibleMoves[i].x;
				float tempY = possibleMoves[i].y;
				for(int j = 0; j < path.Count - 1; j++){
					if(path[i].x == tempX && path[i].y == tempY){
						//do nothing
					}else{
						nonBlockedTiles.Add(possibleMoves[i]);
					}
				}
					
			}
		}
		return nonBlockedTiles;

	}
	

}
