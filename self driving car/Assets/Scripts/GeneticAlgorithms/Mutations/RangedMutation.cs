using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMutation : Mutation {

	float mutationRate;

	public RangedMutation(NeuralNetwork ancestorNetwork, NeuralController controller, float mutationRate)
	{
		this.ancestorNetwork = ancestorNetwork;
		this.controller = controller;
		this.mutationRate = mutationRate;
	}

	public override NeuralNetwork Mutate()
	{
		 NeuralNetwork mutatedNetwork = base.Mutate();
		 return mutatedNetwork;
	}

	protected override MutationData MutateNode(NeuralNode node, NeuralNode ancestorNode)
	{

		MutationData data = new MutationData();

		node.bias = (ancestorNode.bias + Random.Range(-mutationRate, mutationRate)) / 2;

		for(int i = 0; i < node.weights.Length; i++)
		{

			float ancestorWeight = ancestorNode.weights[i];

			float mutation = Random.Range(-mutationRate, mutationRate);

			node.weights[i] = Mathf.Clamp01(ancestorWeight + mutation);

			data.mutationRate += mutation;
			data.affectedWeights++;
		}
		return data;
	}
}
