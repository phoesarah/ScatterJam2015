using UnityEngine;
using System.Collections;

public class BirdSpawn : MonoBehaviour {

    public GameObject _bird = null;
    public int _birdCount = 50;
    public float _birdRadius = 300.0f;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < _birdCount; i++)
        {
            Vector3 v = transform.position + Random.insideUnitSphere * _birdRadius;
            Instantiate(_bird, v, Quaternion.identity);
        }
	}
}
