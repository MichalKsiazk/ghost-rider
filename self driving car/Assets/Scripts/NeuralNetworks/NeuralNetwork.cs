using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{


    public NeuralLayer inputLayer;
    public List<NeuralLayer> hiddenLayers;
    public NeuralLayer outputLayer;

	public readonly NetworkComposition networkComposition;

	public NeuralDebugger networkDebugger;

	public float score;

    public NeuralNetwork(NetworkComposition networkComposition, bool initDebugger, NeuralController controller)
    {
		this.networkComposition = networkComposition; 
		SetupNetworkArchitecture();

		score = 0;

		if(initDebugger) 
		{
			networkDebugger = controller.gameObject.AddComponent(typeof(NeuralDebugger)) as NeuralDebugger;
			networkDebugger.Init(this, controller.nodeTexture, controller.targetCanvas);
		}
    }

	void SetupNetworkArchitecture()
	{ 

		int nextLayerSize = networkComposition.hiddenLayers[0].nodes;
		inputLayer = new NeuralLayer(networkComposition.inputLayer.nodes, LayerType.INPUT_LAYER, nextLayerSize);

		SetupHiddenLayers();


		outputLayer = new NeuralLayer(networkComposition.outputLayer.nodes, LayerType.OUTPUT_LAYER, -1);

	}


	void SetupHiddenLayers()
	{
		hiddenLayers = new List<NeuralLayer>();

		for(int i = 0; i < networkComposition.hiddenLayers.Count - 1; i++)
		{
			int tct = networkComposition.hiddenLayers[i].nodes;
			int lct = networkComposition.hiddenLayers[i + 1].nodes;
			hiddenLayers.Add(new NeuralLayer(tct, LayerType.HIDDEN_LAYER, lct));		
		}

		int lastHiddenLayerSize = networkComposition.hiddenLayers[networkComposition.hiddenLayers.Count - 1].nodes;
		int outputLayerSize = networkComposition.outputLayer.nodes;
		hiddenLayers.Add(new NeuralLayer(lastHiddenLayerSize, LayerType.HIDDEN_LAYER, outputLayerSize));
	}

	

	public void Action(List<float> values)
	{
		ResetAll();
		FeedInputs(values);
		inputLayer.CalculateOutputs();
		inputLayer.FeedNextLayer(hiddenLayers[0]);

		for(int i = 0; i < hiddenLayers.Count - 1; i++)
		{
			hiddenLayers[i].CalculateOutputs();
			hiddenLayers[i].FeedNextLayer(hiddenLayers[i + 1]);
		}
		hiddenLayers[hiddenLayers.Count - 1].CalculateOutputs();
		hiddenLayers[hiddenLayers.Count - 1].FeedNextLayer(outputLayer);

		outputLayer.CalculateOutputs();

	}

	public void ResetAll()
	{
		inputLayer.Reset();
		foreach(NeuralLayer layer in hiddenLayers)
		{
			layer.Reset();
		}
		outputLayer.Reset();
	}

	public void SetWeightsRandomly()
	{
		inputLayer.SetAllWeightsRandomly();

		foreach(NeuralLayer layer in hiddenLayers)
		{
			layer.SetAllWeightsRandomly();
		}
		
		outputLayer.SetAllWeightsRandomly();
	}

	public void AddScore(float score)
	{
		this.score += score;
		//Debug.Log(this.score);
	}

	#region INPUT_FEEDERS

	public void FeedInputs(List<float> values)
	{
		for(int i = 0; i < inputLayer.nodes.Count; i++)
		{
			inputLayer.nodes[i].Feed(values[i]);
		}
	}

	#endregion
}
