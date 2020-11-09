using UnityEngine;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour {
    public List<GameObject> powerUps;
    private System.Random rand = new System.Random();
    private GameObject spawnedObject;

    void Update() {
        if(rand.Next(5000, 15000) == 5000 && spawnedObject == null) {
            spawnedObject = Instantiate(powerUps[rand.Next(powerUps.Count)]);
        }
    }
}
