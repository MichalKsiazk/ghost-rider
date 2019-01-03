using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LayerType
{
	INPUT_LAYER,
	HIDDEN_LAYER,
	OUTPUT_LAYER
}

public class NeuralLayer
{

    public List<NeuralNode> nodes;
	public LayerType layerType;


    public NeuralLayer(int lenght, LayerType layerType, int nextLayerSize)
    {
		nodes = new List<NeuralNode>();

		this.layerType = layerType;

		for(int i = 0; i < lenght; i++)
		{
			switch(layerType)
			{
				case LayerType.INPUT_LAYER:
					nodes.Add(new NeuralNode(lenght, nextLayerSize, NeuronType.input_neuron));
					break;
				case LayerType.HIDDEN_LAYER:
					nodes.Add(new NeuralNode(lenght, nextLayerSize, NeuronType.hidden_neuron));
					break;
				case LayerType.OUTPUT_LAYER:
					nodes.Add(new NeuralNode(lenght, nextLayerSize, NeuronType.output_neuron));
					break;
			}
			SetAllWeightsRandomly();
		}
    }

	public void SetAllWeightsRandomly()
	{
		foreach(NeuralNode node in nodes)
		{
			node.SetRandomWeights();
			node.bias = Random.Range(0.0f,1.0f);
		}
	}


	public void CalculateOutputs()
	{
		foreach(NeuralNode node in nodes)
		{
			node.CalculateOutputs();
		}
	}

	public void FeedNextLayer(NeuralLayer layer)
	{
		foreach(NeuralNode node in nodes)
		{
			node.FeedNextLayer(layer);
		}
	}

	public void Reset()
	{
		foreach(NeuralNode node in nodes)
		{
			node.Reset();
		}
	}
}
