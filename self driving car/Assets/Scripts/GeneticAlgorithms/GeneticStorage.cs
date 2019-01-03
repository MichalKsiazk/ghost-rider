using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneticStorage 
{

	public List<Generation> generations;

	public GeneticStorage()
	{
		generations = new List<Generation>();
		generations.Add(new Generation());
	}

	public int FindBest(Generation generation)
	{

		int bestScoreIndex = -1;

		float maxScore = 0;

		for(int i = 0; i < generation.genomes.Count; i++)
		{

			float score = generation.genomes[i].score;

			if(score > maxScore)
			{
				maxScore = score;
				bestScoreIndex = i;
			}
		} 

		return bestScoreIndex;
	}

	public NeuralNetwork GetNetwork(int generationIndex, int genomeIndex) 
	{
		return generations[generationIndex].genomes[genomeIndex];
	}

}

public class Generation
{

	public List<NeuralNetwork> genomes;

	public Generation()
	{
		genomes = new List<NeuralNetwork>();
	}
}
