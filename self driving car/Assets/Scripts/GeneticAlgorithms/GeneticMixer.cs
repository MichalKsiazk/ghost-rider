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

	[HideInInspector] public MutationOptions mutationOptions;
	[HideInInspector] public float mutationRate;

	float best = 0;

	[HideInInspector] public bool noRegression;

	void Start() 
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

	void Update() 
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

	public void Mutate()
	{

		NeuralNetwork ancestor = FindBestAncestor();

		switch(mutationOptions)
		{
			case MutationOptions.Random_Mutation:
				controller.neuralNetwork = new RandomMutation(ancestor, controller).Mutate();
				break;
			case MutationOptions.Ranged_Mutation:
				//Debug.Log("Ranged Mutation");
				controller.neuralNetwork = new RangedMutation(ancestor, controller, mutationRate).Mutate();
				break;
		}
	}

	public int LastGenerationIndex()
	{
		return currentGenerationIndex - 1;
	}

	public NeuralNetwork FindBestAncestor()
	{
		int bestFromLastGeneration = geneticStorage.FindBest(geneticStorage.generations[LastGenerationIndex()]);
		return geneticStorage.GetNetwork(LastGenerationIndex(), bestFromLastGeneration);
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

public enum MutationOptions
{
	Random_Mutation,
	Ranged_Mutation
}
