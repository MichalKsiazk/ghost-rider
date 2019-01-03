using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMutation : Mutation
{

	public RandomMutation(NeuralNetwork ancestorNetwork, NeuralController controller)
	{
		this.ancestorNetwork = ancestorNetwork;
		this.controller = controller;
	}

	public override NeuralNetwork Mutate()
	{
		 NeuralNetwork mutatedNetwork = base.Mutate();
		 return mutatedNetwork;
	}

	protected override MutationData MutateNode(NeuralNode node, NeuralNode ancestorNode)
	{

		MutationData data = new MutationData();

		node.bias = (ancestorNode.bias + Random.Range(0.0f, 1.0f)) / 2;

		for(int i = 0; i < node.weights.Length; i++)
		{
			float mutation = Random.Range(0.0f, 1.0f); 

			float ancestorWeight = ancestorNode.weights[i];

			node.weights[i] = (ancestorWeight + mutation) / 2;

			data.mutationRate += mutation;
			data.affectedWeights++;
		}
		return data;
	}
	
}
