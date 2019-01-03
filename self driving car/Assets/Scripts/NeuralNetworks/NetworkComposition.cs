using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkComposition 
{


	public LayerComposition inputLayer;
	public List<LayerComposition> hiddenLayers;
	public LayerComposition outputLayer;


	public NetworkComposition(int inputNodes, int outputNodes)
	{

		//Debug.Log(string.Format("({inputNodes}, {outputNodes})"));

		hiddenLayers = new List<LayerComposition>();

		inputLayer = new LayerComposition(inputNodes, LayerType.INPUT_LAYER);
		outputLayer = new LayerComposition(outputNodes, LayerType.OUTPUT_LAYER);
	}

	public void AddHiddenLayer(int nodes)
	{
		hiddenLayers.Add(new LayerComposition(nodes, LayerType.HIDDEN_LAYER));
	}

	public void AddHiddenLayers(int layers, int nodes)
	{
		for(int i = 0; i < layers; i++)
		{
			AddHiddenLayer(nodes);
		}
	}

}

public class LayerComposition
{

	public int nodes;
	public LayerType layerType;


	public LayerComposition(int nodes, LayerType layerType)
	{
		this.layerType = layerType;
		this.nodes = nodes;
	}
}
