using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour {
    public static FoodSpawn Reference;
    public GameObject Food;
	// Use this for initialization
	void Awake () {
        Reference = this;
        Spawn();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("f"))
        Spawn();
	}
     public void Spawn() {

        Vector3 spawnPoint = Vector3.zero;
       
            spawnPoint = new Vector3(Random.Range((int)-9, 9), 0, Random.Range((int)-9, 9));
        if (!Physics.Raycast(spawnPoint + Vector3.up * 10, Vector3.down))
            Instantiate(Food, spawnPoint, Quaternion.identity);
        else
            Spawn();
    }

}
