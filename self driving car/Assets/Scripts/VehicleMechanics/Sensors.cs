using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour 
{
	[SerializeField] public Sensor[] sensors;

	[Range(0,90)] public float rot;


	private Color debugSpheresColor;

	[HideInInspector] public bool vehicleTouchingWalls;

	[HideInInspector] public bool vehicleCrashed;

	public float vehicleSpeed;

	Vector3 previousVehiclePosition;

	void Start () 
	{
		previousVehiclePosition = transform.position;
		vehicleTouchingWalls = false;
		vehicleCrashed = false;

		debugSpheresColor = Color.green;

		GameObject sensorParent = new GameObject();

		sensorParent.transform.parent = this.transform;

	

		for(int i = 0; i < sensors.Length; i++)
		{
			SetupSensor(i, sensorParent);
		}
	}
	
	void Update () 
	{
		CalculateVehicleSpeed();
	}

	void CalculateVehicleSpeed()
	{
		vehicleSpeed = Vector3.Distance(transform.position, previousVehiclePosition);
		previousVehiclePosition = transform.position;
	}


	void SetupSensor(int index, GameObject parentObject)
	{

	}

	void OnDrawGizmos()
    {
		for(int i = 0; i < sensors.Length; i++)
		{
			Vector3 root_position = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + rot, transform.rotation.eulerAngles.z) * new Vector3(sensors[i].x_shift, sensors[i].y_shift, sensors[i].z_shift);

			Gizmos.color = debugSpheresColor;
			Gizmos.DrawSphere(transform.position + root_position, 0.1f);
			

			Vector3 target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + sensors[i].angle, transform.eulerAngles.z) * new Vector3(1,0,1) * sensors[i].ray_dst;
			//Gizmos.DrawLine(transform.position + root_position,transform.position + root_position + target);

			Gizmos.color = debugSpheresColor;
			Gizmos.DrawSphere(transform.position + root_position + target, 0.2f);

			RaycastHit hit;

			if (Physics.Raycast(transform.position + root_position, target, out hit, sensors[i].ray_dst))
        	{

				float red = 1.1f - (((hit.distance * 100) / sensors[i].ray_dst) / 100);
				float green = 0.1f + ((hit.distance * 100) / sensors[i].ray_dst) / 100;

				Color line_color = new Color(red, green, 0, 0.5f);

				Gizmos.color = line_color;
           		Gizmos.DrawLine(transform.position + root_position, hit.point);
				
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine(hit.point, transform.position + root_position + target);

				Gizmos.color = debugSpheresColor;
				Gizmos.DrawSphere(hit.point, 0.1f);

				sensors[i].raycastDistance = hit.distance;

        	}
        	else
        	{
				Gizmos.DrawLine(transform.position + root_position, transform.position + root_position + target);
        	}
		}
    }

	public void RaycastSensor(Sensor sensor)
	{

	}

	void OnCollisionEnter(Collision collision)
    {
        debugSpheresColor = Color.red;
		vehicleTouchingWalls = true;
		vehicleCrashed = true;
    }

	void OnCollisionExit(Collision collision) 
	{
		debugSpheresColor = Color.green;
		vehicleTouchingWalls = false;
	}

	public float[] GetSensorInputs()
	{
		float[] sensorInputs = new float[sensors.Length];
		for(int i = 0; i < sensors.Length; i++)
		{
			sensorInputs[i] = sensors[i].raycastDistance;
		} 
		return sensorInputs;
	}

	public bool Restart()
	{
		vehicleCrashed = false;
		return true;
	}

}

[System.Serializable]
public class Sensor
{
	[Range(-4,4)] public float x_shift;

	[Range(-4,4)] public float y_shift;

	[Range(-4,4)] public float z_shift;

	[Range(-90, 90)] public float angle;

	[Range(0,30f)] public float ray_dst;

	[HideInInspector] public Vector3 root_pos;
	[HideInInspector] public Vector3 ray_target;

	public float raycastDistance;

}
