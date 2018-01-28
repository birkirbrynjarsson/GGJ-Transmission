using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpanwer : MonoBehaviour {

	public int maxObjects;
	public GameObject[] spawnableObjects;
	public float minX, maxX, minZ, maxZ;
	public float spawnHeight, destroyHeight;
	public float spawnTimer;
	public float destroyTimer;
	public float currentTimer = -1;
	private System.Random rand;
	private List<GameObject> spawnedObjects;

	// Use this for initialization
	void Start () {
		rand = new System.Random((int)System.DateTime.Now.Ticks & 0x0000FFFF);
		spawnedObjects = new List<GameObject>();
		StartCoroutine(Spawn());
		StartCoroutine(Destroy());
	}

	private IEnumerator Spawn() {
		while (true) {
			if (spawnedObjects.Count < maxObjects) {
				SpawnRandomObject();
			}
			yield return new WaitForSeconds(spawnTimer);
		}
	}

	private IEnumerator Destroy() {
		while (true) {
			List<GameObject> toRemove = new List<GameObject>();
			foreach (GameObject obj in spawnedObjects) {
				if (obj.transform.position.y < destroyHeight) {
					toRemove.Add(obj);
				}
			}

			foreach (GameObject obj in toRemove) {
				spawnedObjects.Remove(obj);
				Destroy(obj);
			}
			yield return new WaitForSeconds(destroyTimer);
		}
	}

	private void SpawnRandomObject() {
		spawnedObjects.Add((GameObject)Instantiate(GetRandomObject(), GetRandomPosition(), Quaternion.Euler((float)rand.NextDouble() * 360, (float)rand.NextDouble() * 360, (float)rand.NextDouble() * 360)));
	}

	private Vector3 GetRandomPosition() {
		return new Vector3((float)rand.NextDouble()*(maxX-minX)+minX, spawnHeight, (float)rand.NextDouble()*(maxZ-minZ)+minZ);
	}

	private GameObject GetRandomObject() {
		return spawnableObjects[rand.Next(spawnableObjects.Length)];
	}
}
