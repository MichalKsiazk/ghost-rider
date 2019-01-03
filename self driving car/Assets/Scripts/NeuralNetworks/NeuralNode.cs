using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeuronType
{
	input_neuron,
	hidden_neuron,
	output_neuron
}


public class NeuralNode
{

    public float bias;
    public float[] weights;

    public float data;

	public float[] outputs;

    public string name;

	public readonly NeuronType type;

    public NeuralNode(int layerSize, int nextLayerSize, NeuronType neuronType)
    {

		this.type = neuronType;
        this.bias = Random.Range(0.0f, 1.0f);
		//this.bias = 0;
		if(neuronType == NeuronType.output_neuron)
		{
			this.weights = new float[layerSize];
			this.outputs = new float[1];
		}
		else
		{
			//TODO: warstwa wyjściowa powinna nie mieć wag i mieć tyle outputów co nodeów

			this.weights = new float[nextLayerSize];
			this.outputs = new float[nextLayerSize];
		}
        Reset();
    }

	public NeuralNode()
	{

	}

    public void Reset()
    {
        data = 0;
    }

    public void Feed(float input)
    {
		data += input;
    }

	public void CalculateOutputs()
	{
		if(type != NeuronType.output_neuron)
		{
			for(int i = 0; i < outputs.Length; i++)
			{
				outputs[i] = NeuralUtils.CustomSigmoid(bias + data * weights[i], -0.5f, 2, 0.3f);
			}
		}
		else
		{
				outputs[0] = NeuralUtils.CustomSigmoid(bias + data, -0.5f, 2, 0.3f);
		}
	}

	public void FeedNextLayer(NeuralLayer nextLayer)
	{

		for(int i = 0; i < nextLayer.nodes.Count; i++)
		{
			nextLayer.nodes[i].Feed(outputs[i]);
		}
	}

	public void SetRandomWeights()
	{
		for(int i = 0; i < weights.Length; i++)
		{
			weights[i] = Random.Range(0.0f, 1.0f);
		}
	}

	public void Mutate_1(int weightIndex, float ancestorWeight)
	{
		float mutation = Random.Range(0.0f, 1.0f); 

		weights[weightIndex] = (ancestorWeight + mutation) / 2;
	}

	public float Mutate_2(int weightIndex, float ancestorWeight, float mutationRate)
	{

		float mutation = Random.Range(-mutationRate, mutationRate);

		weights[weightIndex] = Mathf.Clamp01(ancestorWeight + mutation);

		return mutation;
	}
}