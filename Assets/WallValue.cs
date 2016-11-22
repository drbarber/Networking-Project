using UnityEngine;
using System.Collections;

public class WallValue : MonoBehaviour {
    public float value;
	// Use this for initialization
	void Awake () {
        value =  Random.Range(0, 100);
        this.gameObject.tag.Equals(value);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
