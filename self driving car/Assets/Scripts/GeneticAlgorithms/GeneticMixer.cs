using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneticMixer : MonoBehaviour {

	[Range(0,20)]
	public int generationSize;
	
	public int currentGenome;

	bool componentInitialized;

	public GeneticStorage geneticStorage;

	NeuralController controller;

	public int currentGenerationIndex;

	Canvas targetCanvas;
	RectTransform backgroundPanel;
	Text outputText;

	float best = 0;

	void Start () 
	{
		currentGenome = 0;
		currentGenerationIndex = 0;
		componentInitialized = false;
		targetCanvas = GameObject.FindGameObjectWithTag("ui_canvas").GetComponent<Canvas>();
		SetupPanel(backgroundPanel);
		SetupTextLabel();
	}

	public void Init()
	{
		geneticStorage = new GeneticStorage();
		componentInitialized = true;
	}

	void Update () 
	{
		WriteInfo();
	}

	void SetupPanel(RectTransform panel)
	{

		float size = 300;

		GameObject panelObject = new GameObject("background_panel");

		panelObject.transform.parent = targetCanvas.transform;
		panel = panelObject.AddComponent<RectTransform>();
		panel.sizeDelta = new Vector2 (size, size);
		Image backgroundImage = panel.gameObject.AddComponent<Image>();
		backgroundImage.color = new Color(0.2f, 0.2f, 0.2f, 0.4f);

		panel.localPosition = new Vector2(-size * 0.5f, size * 0.5f);
		panel.anchorMin = new Vector2(1, 0);
		panel.anchorMax = new Vector2(1, 0);
	}

	void SetupTextLabel()
	{

		float size = 300;

		GameObject textLabelObject = new GameObject("network_data");
		textLabelObject.transform.SetParent(targetCanvas.transform);
		RectTransform textPanelTransform = textLabelObject.AddComponent<RectTransform>();

		textPanelTransform.localPosition = new Vector2(-size * 0.5f, size * 0.5f);
		textPanelTransform.anchorMin = new Vector2(1, 0);
		textPanelTransform.anchorMax = new Vector2(1, 0);
		textPanelTransform.sizeDelta = new Vector2 (size, size);

		outputText = textLabelObject.AddComponent<Text>();
		outputText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		outputText.fontSize = 20;
	}

	void WriteInfo()
	{


		string info = "GENERATION: " + (currentGenerationIndex + 1).ToString() + "\n\n";
		info += "GENOME: " + (currentGenome + 1).ToString() + "\n\n";
		info += "SCORE: " + controller.neuralNetwork.score + "\n\n";
		info += "SPEED: " + controller.vehicle.velocity + "\n\n";

		if(controller.neuralNetwork.score > best)
		{
			best = controller.neuralNetwork.score;
		}

		info += "BEST: " + best;

		outputText.text = info;
		
		//Debug.Log("text written");

	}
	
	public bool ConnectWithController(NeuralController controller, out GeneticMixer mixer)
	{
		this.controller = controller;
		mixer = this;
		return true;
	}

	public NeuralNetwork Mutate_1(NeuralNetwork ancestorNetwork, NeuralController _controller)
	{

		NeuralNetwork mixedNetwork = new NeuralNetwork(ancestorNetwork.networkComposition, true, _controller);

		mixedNetwork.inputLayer.Mutate_1(ancestorNetwork.inputLayer);

		for(int i = 0; i < mixedNetwork.hiddenLayers.Count; i++)
		{
			mixedNetwork.hiddenLayers[i].Mutate_1(ancestorNetwork.hiddenLayers[i]);
		}

		mixedNetwork.outputLayer.Mutate_1(ancestorNetwork.outputLayer);

		return mixedNetwork;
	}

	public NeuralNetwork Mutate_2(NeuralNetwork ancestorNetwork, NeuralController _controller, float mutationRate)
	{

		MutationData data = new MutationData();

		NeuralNetwork mixedNetwork = new NeuralNetwork(ancestorNetwork.networkComposition, true, _controller);

		data += mixedNetwork.inputLayer.Mutate_2(ancestorNetwork.inputLayer, mutationRate);

		for(int i = 0; i < mixedNetwork.hiddenLayers.Count; i++)
		{
			data += mixedNetwork.hiddenLayers[i].Mutate_2(ancestorNetwork.hiddenLayers[i], mutationRate);
		}

		data += mixedNetwork.outputLayer.Mutate_2(ancestorNetwork.outputLayer, mutationRate);
		Debug.Log("MUTATION LOG " + data.AverageMutation().ToString());
		return mixedNetwork;
	}

	public void SaveGenome(NeuralNetwork neuralNetwork)
	{
		geneticStorage.generations[currentGenerationIndex].genomes.Add(neuralNetwork);
		currentGenome++;
		if(currentGenome >= generationSize)
		{
			currentGenome = 0;
			currentGenerationIndex++;
			geneticStorage.generations.Add(new Generation());
		}
	}
}



public class MutationData
	{
		public float mutationRate;
		public float affectedWeights;
		
		public MutationData()
		{
			mutationRate = 0;
			affectedWeights = 0;
		}

		public static MutationData operator+ (MutationData a, MutationData b)
		{

			MutationData addedData = new MutationData();

			addedData.mutationRate = a.mutationRate + b.mutationRate;
			addedData.affectedWeights = a.affectedWeights + b.affectedWeights;

			return addedData;
		}

		public float AverageMutation()
		{
			return mutationRate / affectedWeights;
		}
	}
