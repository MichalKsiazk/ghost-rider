using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutation {

	public MutationData mutationData;

	protected NeuralNetwork ancestorNetwork;
	protected NeuralController controller;

	public Mutation()
	{

	}
	
	public virtual NeuralNetwork Mutate()
	{
		NeuralNetwork mixedNetwork = new NeuralNetwork(ancestorNetwork.networkComposition, true, controller);

		MutationData data = new MutationData();


		data += MutateLayer(mixedNetwork.inputLayer, ancestorNetwork.inputLayer);

		for(int i = 0; i < mixedNetwork.hiddenLayers.Count; i++)
		{
			data += MutateLayer(mixedNetwork.hiddenLayers[i], ancestorNetwork.hiddenLayers[i]);
		}

		data += MutateLayer(mixedNetwork.outputLayer, ancestorNetwork.outputLayer);

		return mixedNetwork;
	}

	protected MutationData MutateLayer(NeuralLayer targetLayer, NeuralLayer ancestorLayer)
	{

		MutationData data = new MutationData();

		for(int n = 0; n < targetLayer.nodes.Count; n++)
		{
			data += MutateNode(targetLayer.nodes[n], ancestorLayer.nodes[n]);				
		}
		return data;
	}

	protected virtual MutationData MutateNode(NeuralNode node, NeuralNode ancestorNode)
	{
		return null;
	}

}
