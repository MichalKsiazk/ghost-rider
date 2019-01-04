using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneticMixer))]
public class GeneticMixerEditor : Editor 
{
	void OnEnable()
	{

	}

	public override void OnInspectorGUI()
    {
		GeneticMixer mixer = (GeneticMixer)target;

		mixer.generationSize = EditorGUILayout.IntSlider("Generation Size", mixer.generationSize, 1, 30);
		mixer.mutationRate = EditorGUILayout.Slider("Mutation Rate", mixer.mutationRate, 0, 1.0f);

		mixer.mutationOptions = (MutationOptions)EditorGUILayout.EnumPopup(mixer.mutationOptions);
		mixer.noRegression = EditorGUILayout.Toggle("No Regression", mixer.noRegression);
    }
}
