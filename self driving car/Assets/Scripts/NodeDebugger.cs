using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeDebugger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	
	NeuralNode node;

	public void Init(NeuralNode node)
	{
		this.node = node;
	}


	public void OnPointerEnter(PointerEventData pointerEventData)
    {

		string weights = "";

		for(int i = 0; i < node.weights.Length; i++)
		{
			weights += node.weights[i].ToString() + " ";
		}

        Debug.Log(weights);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
 
    }


}
