using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NeuralController))]

public class NeuralControllerEditor : Editor 
{

    bool showLayerConfig;
    bool showCompositionConfig;

	void OnEnable()
    {

        NeuralController controller = (NeuralController)target;

        showLayerConfig = false;
        showCompositionConfig = false;
        if(controller.hiddenLayers == null)
        {
            controller.hiddenLayers = new List<int>();
        }
    }

    public override void OnInspectorGUI()
    {
        NeuralController controller = (NeuralController)target;

        showCompositionConfig = EditorGUILayout.Foldout(showCompositionConfig, "Composition");
        if(showCompositionConfig)
        {
            ShowCompositionConfig(controller);
        }
        base.OnInspectorGUI();
    }

    void ShowCompositionConfig(NeuralController controller)
    {

        controller.inputNodes = EditorGUILayout.IntField("Input nodes:", controller.inputNodes);
        controller.outputNodes = EditorGUILayout.IntField("Output nodes:", controller.outputNodes);
        ShowLayerConfig(controller);
        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();
    }

    void ShowLayerConfig(NeuralController controller)
    {

        showLayerConfig = EditorGUILayout.Foldout(showLayerConfig, "Hidden Layers");

        if(showLayerConfig) 
        {
            int lenght = controller.hiddenLayers.Count;
            for(int i = 0; i < lenght; i++)
            {
                controller.hiddenLayers[i] = EditorGUILayout.IntField(controller.hiddenLayers[i]);
            }
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("+"))
            {
                controller.hiddenLayers.Add(0);
            }
            if(GUILayout.Button("-"))
            {
                if(lenght >= 1)
                {
                    controller.hiddenLayers.RemoveAt(lenght - 1);
                }
            }
            GUILayout.EndHorizontal();
        }

    }
}
