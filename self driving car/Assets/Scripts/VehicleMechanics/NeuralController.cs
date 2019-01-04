using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralController : MonoBehaviour {

	private Sensors sensors;

	private float[] sensorInputs;


	public NeuralNetwork neuralNetwork;
	NetworkComposition networkComposition;

	[HideInInspector] public Vehicle vehicle;

	private GeneticMixer mixer;

	[Header("Network Debbuger Settings")]
		public Sprite nodeTexture;
		public Canvas targetCanvas;

	bool mixerConnected;

	[HideInInspector] public int inputNodes;
	[HideInInspector] public List<int> hiddenLayers;
	[HideInInspector] public int outputNodes;

	void Start () 
	{
		mixerConnected = false;
		vehicle = this.gameObject.GetComponent<Vehicle>();

		sensors = GetComponent<Sensors>();
		sensorInputs = new float[sensors.sensors.Length];

		networkComposition = Composition();
		neuralNetwork = new NeuralNetwork(networkComposition, true, this);

		bool mixerFound = FindAttachedMixer(this.gameObject);

		if(mixerFound)
		{
			mixerConnected = mixer.ConnectWithController(this, out mixer);
			mixer.Init();
		}
	}
	
	void Update () 
	{
		sensorInputs = sensors.GetSensorInputs();

		float steer = neuralNetwork.outputLayer.nodes[1].outputs[0];

		steer = Mathf.Clamp(steer, -1.0f, 1.0f);

		List<float> packedSensorInputs = SerializedInputData(sensorInputs, sensors.vehicleSpeed);
		neuralNetwork.Action(packedSensorInputs);
		if(vehicle.velocity < vehicle.maxVelocity)
		{
			vehicle.Accelerate(neuralNetwork.outputLayer.nodes[0].outputs[0]);
		}
		vehicle.Steer(steer * 10);
		//Debug.Log(steer);
	}

	void FixedUpdate()
	{
		neuralNetwork.AddScore(vehicle.velocity * 0.1f);
	}

	NetworkComposition Composition()
	{
		NetworkComposition composition = new NetworkComposition(sensors.sensors.Length, outputNodes);
		for(int i = 0; i < hiddenLayers.Count; i++)
		{
			composition.AddHiddenLayer(hiddenLayers[i]);
		}
		return composition;
	}

	List<float> SerializedInputData(float[] sensorInputs, float speed)
	{
		List<float> data = new List<float>();
		for(int i = 0; i < sensors.sensors.Length; i++)
		{			
			data.Add(sensorInputs[i]);
		}
		return data;
	}

	public void OnReset()
	{
		neuralNetwork.networkDebugger.ShutOff();
		Destroy(neuralNetwork.networkDebugger);
		mixer.SaveGenome(neuralNetwork);

		if(mixer.currentGenerationIndex == 0)
		{
			neuralNetwork = new NeuralNetwork(networkComposition, true, this);
		}
		else
		{

			if(mixer.noRegression && mixer.currentGenome == 0)
			{
				Debug.Log("ancestor copied without mutation");
				neuralNetwork = mixer.FindBestAncestor();
			}
			else
			{
				mixer.Mutate();
			}
		}
	}

	void LogBestNetwork()
	{

		int bestNetworkIndex = mixer.geneticStorage.FindBest(mixer.geneticStorage.generations[0]);
		float networkScore = mixer.geneticStorage.generations[0].genomes[bestNetworkIndex].score;

		Debug.Log(string.Format("BEST INDEX {0}, SCORE {1}", bestNetworkIndex, networkScore));
	}

	bool FindAttachedMixer(GameObject target)
	{
		mixer = target.GetComponent<GeneticMixer>();
		if(mixer != null)
		{
			return true;
		}
		Debug.Log("mixer not found");
		return false;
	}
}
