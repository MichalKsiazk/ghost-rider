using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ManualController : MonoBehaviour {

	Vehicle vehicle;

	void Start()
	{
		vehicle = this.gameObject.GetComponent<Vehicle>();
	}

	void FixedUpdate()
	{

		vehicle.Brake(Input.GetKey(KeyCode.Space));
		vehicle.Steer(Input.GetAxis("Horizontal"));
		vehicle.Accelerate(Input.GetAxis("Vertical"));

	}
}
