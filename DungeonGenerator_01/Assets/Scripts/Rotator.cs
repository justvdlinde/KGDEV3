using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    private Quaternion rot = Quaternion.Euler(90, 0, 0);
    private GameObject[] rooms;

    void Start() {
        rooms = GameObject.FindGameObjectsWithTag("Map");
        foreach (GameObject go in rooms) {
            go.transform.rotation = rot;
        }
    }
}
