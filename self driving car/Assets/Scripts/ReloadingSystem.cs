using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingSystem : MonoBehaviour 
{

	public GameObject[] targetVehicles;

	VehicleComponents[] vehicleComponents;

	void Start() 
	{
		vehicleComponents = new VehicleComponents[targetVehicles.Length];

		for(int i = 0; i < targetVehicles.Length; i++)
		{

			vehicleComponents[i] = new VehicleComponents();
			vehicleComponents[i].vehicleSensors = targetVehicles[i].GetComponent<Sensors>();
			vehicleComponents[i].vehicle = targetVehicles[i].GetComponent<Vehicle>();
			vehicleComponents[i].neuralController = targetVehicles[i].GetComponent<NeuralController>();
		}
	}
	
	void Update() 
	{


		for(int i = 0; i < targetVehicles.Length; i++)
		{
			if(targetVehicles[i] == null)
			{
				return;
			}


			if(vehicleComponents[i].vehicleSensors.vehicleCrashed)
			{
				Restart(i);
			}
		}
	}

	bool Restart(int i)
	{
		vehicleComponents[i].vehicleSensors.Restart();
		vehicleComponents[i].vehicle.Reset();
		vehicleComponents[i].neuralController.OnReset();
		return false;
	}

	class VehicleComponents
	{
		public Sensors vehicleSensors;
		public Vehicle vehicle;
		public NeuralController neuralController;
	}
}
