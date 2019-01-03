using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Vehicle : MonoBehaviour {

	public enum DriveType
	{
		_4x4,
		REAR_AXIS,
		FRONT_AXIS
	}

	private float m_steeringAngle;

	public WheelCollider frontLeftWheel, frontRightWheel;
	public WheelCollider backLeftWheel, backRightWheel;
	public Transform frontLeftTransform, frontRightTransform;
	public Transform backLeftTransform, backRightTransform;
	public float maxSteerAngle = 30;
	public float motorForce = 50;	
	public float velocity;
	
	[Range(100,10000)]
	public float maxRPM;

	[Range(0,10)]
	public float maxVelocity;

	public DriveType driveType;

	private Vector3 lastFramePosition;

	public Image speedometer;

	float powerLimit;

	bool carIsBraking;

	bool lockedSpeedMeasure;

	public Vector3 startPosition;
	public Quaternion startDirection;

	void Start () 
	{
		lastFramePosition = transform.position;
		speedometer = GameObject.Find("Speedometer").GetComponent<Image>();
		powerLimit = 0;
		carIsBraking = false;
		lockedSpeedMeasure = false;

		startPosition = transform.position;
		startDirection = transform.rotation;
	}
	
	void FixedUpdate () 
	{
		CalculateVelocity();

		UpdateWheelPoses();
		powerLimit = velocity * 1000;
		ShowSpeed();

	}

	void CalculateVelocity()
	{
		if(!lockedSpeedMeasure)
		{
			velocity = Vector3.Distance(lastFramePosition, transform.position);
		}
		else
		{
			lockedSpeedMeasure = false;
		}
		
		lastFramePosition = transform.position;
	}

	public void Steer(float horizontalInput)
	{
		m_steeringAngle = maxSteerAngle * horizontalInput;
		frontLeftWheel.steerAngle = m_steeringAngle;
		frontRightWheel.steerAngle = m_steeringAngle;
	}

	public void Brake(bool brake)
	{
		if( Input.GetKey(KeyCode.Space))
		{
			frontLeftWheel.brakeTorque += 100;
			frontRightWheel.brakeTorque += 100;

			backLeftWheel.brakeTorque += 100;
			backRightWheel.brakeTorque += 100;
			carIsBraking = true;
		}
		else
		{
			frontLeftWheel.brakeTorque = 0;
			frontRightWheel.brakeTorque = 0;

			backLeftWheel.brakeTorque = 0;
			backRightWheel.brakeTorque = 0;
			carIsBraking = false;
		}
	}

	public void Accelerate(float verticalInput)
	{
		if(velocity < maxVelocity && frontLeftWheel.rpm < maxRPM && !carIsBraking)
		{
			if(driveType == DriveType._4x4 || driveType == DriveType.FRONT_AXIS)
			{
				frontLeftWheel.motorTorque = verticalInput * motorForce;
				frontRightWheel.motorTorque = verticalInput * motorForce;
			}
			if(driveType == DriveType._4x4 || driveType == DriveType.REAR_AXIS)
			{
				backLeftWheel.motorTorque = verticalInput * motorForce;
				backRightWheel.motorTorque = verticalInput * motorForce;
			}
		}

		if(frontLeftWheel.rpm > maxRPM)
		{
			frontLeftWheel.motorTorque = 0;
		}
		if(frontRightWheel.rpm > maxRPM)
		{
			frontRightWheel.motorTorque = 0;
		}
	}

	private void ShowSpeed()
	{
		float fillAmount = (velocity * 100 / maxVelocity) * 0.01f;
		speedometer.fillAmount = fillAmount;
	}

	public void UpdateWheelPoses()
	{
		UpdateWheelPose(frontLeftWheel, frontLeftTransform);
		UpdateWheelPose(frontRightWheel, frontRightTransform);
		UpdateWheelPose(backLeftWheel, backLeftTransform);
		UpdateWheelPose(backRightWheel, backRightTransform);
	}

	public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 _pos = _transform.position;
		Quaternion _quat = _transform.rotation;

		_collider.GetWorldPose(out _pos, out _quat);

		_transform.position = _pos;
		_transform.rotation = _quat;
	}

	public void Reset()
	{		

		transform.position = startPosition;
		transform.rotation = startDirection;

		velocity = 0;
		frontLeftWheel.motorTorque = 0;
		frontRightWheel.motorTorque = 0;
		backLeftWheel.motorTorque = 0;
		backRightWheel.motorTorque = 0;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		lockedSpeedMeasure = true;

	}
}
