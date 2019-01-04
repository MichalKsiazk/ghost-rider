using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralDebugger : MonoBehaviour
{


	NeuralNetwork network;

	bool script_initialized;

	Camera mainCamera;

	Sprite nodeTexture;
	Canvas targetCanvas;

	List<NodeRenderer> inputLayer;
	List<List<NodeRenderer>> hiddenLayers;
	List<NodeRenderer> outputLayer;

	float nodeSize;

	void Start()
	{
		script_initialized = false;
	}

	public void Init(NeuralNetwork network, Sprite nodeTexture, Canvas targetCanvas)
	{
		if(!script_initialized)
		{
			this.targetCanvas = targetCanvas;
			this.nodeTexture = nodeTexture;

			inputLayer = new List<NodeRenderer>();
			hiddenLayers = new List<List<NodeRenderer>>();
			outputLayer = new List<NodeRenderer>();

			nodeSize = 40;

			mainCamera = Camera.main;
			this.network = network;
			script_initialized = true;
			DrawNetwork();
			DebugWeights();
		}
	}

	void Update()
	{
		DebugNodeData();
	}

	public void DrawNetwork()
	{

		float leftMargin = 50;

		float topMargin = 150;


		DrawLayer(network.inputLayer, inputLayer, ref leftMargin, topMargin);
		for(int i = 0; i < network.hiddenLayers.Count; i++)
		{
			hiddenLayers.Add(new List<NodeRenderer>());

			DrawLayer(network.hiddenLayers[i], hiddenLayers[i], ref leftMargin, topMargin);
		}
		DrawLayer(network.outputLayer, outputLayer, ref leftMargin, topMargin);
		DrawConnections(inputLayer, hiddenLayers[0]);

		for(int i = 0; i < hiddenLayers.Count - 1; i++)
		{
			DrawConnections(hiddenLayers[i], hiddenLayers[i + 1]);
		}
		DrawConnections(hiddenLayers[hiddenLayers.Count - 1], outputLayer);

	}


	void DrawLayer(NeuralLayer neuralLayer, List<NodeRenderer> debuggerLayer, ref float leftMargin, float topMargin)
	{

		float verticalShift = 0;

		if(neuralLayer.nodes.Count % 2 == 0)
		{
			topMargin += 25;
		}

		for(int i = 0; i < neuralLayer.nodes.Count; i++)
		{

			GameObject node = new GameObject("node" + i.ToString());
			NodeRenderer nodeRenderer = new NodeRenderer(node.AddComponent<Image>() as Image);
			nodeRenderer.parent = new GameObject("node" + i.ToString());
			NodeDebugger nodeDebugger = nodeRenderer.parent.AddComponent<NodeDebugger>();
			nodeDebugger.Init(neuralLayer.nodes[i]);

			nodeRenderer.parent.transform.SetParent(targetCanvas.transform);
			node.transform.SetParent(nodeRenderer.parent.transform);

			nodeRenderer.nodeImage.sprite = nodeTexture;
			nodeRenderer.nodeImage.rectTransform.sizeDelta = new Vector2(nodeSize, nodeSize);

			float targetCanvasHeight = targetCanvas.gameObject.GetComponent<RectTransform>().rect.height;

			float v = i % 2 == 0 ? -verticalShift : verticalShift; 

			nodeRenderer.nodeImage.rectTransform.position = new Vector2(leftMargin, targetCanvasHeight - topMargin + v);
			nodeRenderer.nodeImage.color = new Color32(48, 112, 181, 255);
			debuggerLayer.Add(nodeRenderer);

			if(i % 2 == 0)
			{
				verticalShift += 60;
			}

			SetupTextLabel(node, nodeRenderer);

		}
		leftMargin += 100;
	}

	void SetupTextLabel(GameObject nodeObject, NodeRenderer target)
	{
		GameObject textLabel = new GameObject("text");
		Text text = textLabel.AddComponent<Text>() as Text;
		textLabel.transform.SetParent(target.parent.transform);
		text.rectTransform.position = target.nodeImage.rectTransform.position;
		text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text.text = "x";
		target.text = text;
		text.alignment = TextAnchor.MiddleCenter;
	}

	void DrawConnections(List<NodeRenderer> layer1, List<NodeRenderer> layer2)
	{
		for(int i1 = 0; i1 < layer1.Count; i1++)
		{
			Vector2 a = layer1[i1].nodeImage.rectTransform.position;
			for(int i2 = 0; i2 < layer2.Count; i2++)
			{
				Vector2 b = layer2[i2].nodeImage.rectTransform.position;
				GameObject obj = new GameObject("connection" + i2.ToString());
				Image connection = obj.AddComponent<Image>() as Image;

				float lenght = Vector2.Distance(a,b);


				float tg = (a.y - b.y) / (a.x -b.x);

				float angle = Mathf.Atan(tg) * Mathf.Rad2Deg;

				float h = targetCanvas.gameObject.GetComponent<RectTransform>().rect.height;
				float v = targetCanvas.gameObject.GetComponent<RectTransform>().rect.width;

				Vector2 midpoint = new Vector2(((a.x + b.x) / 2), ((a.y + b.y) / 2));



				connection.rectTransform.sizeDelta = new Vector2(lenght - nodeSize, 2);
				connection.rectTransform.position = midpoint;

				obj.transform.SetParent(layer1[i1].parent.transform);
				layer1[i1].weights.Add(connection);
				connection.rectTransform.Rotate(0,0, angle);
				connection.color = Color.blue;

			}
		}
	}

	void DebugNodeData()
	{
		for(int i = 0; i < inputLayer.Count; i++)
		{
			inputLayer[i].text.text = network.inputLayer.nodes[i].data.ToString("F2");
		}
		for(int i = 0; i < hiddenLayers.Count; i++)
		{
			for(int n = 0; n < hiddenLayers[i].Count; n++)
			{
				hiddenLayers[i][n].text.text = network.hiddenLayers[i].nodes[n].data.ToString("F2");
			}
		}
		for(int i = 0; i < outputLayer.Count; i++)
		{
			outputLayer[i].text.text = network.outputLayer.nodes[i].data.ToString("F2");
		}
	}

	void DebugWeights()
	{
		for(int i = 0; i < inputLayer.Count; i++)
		{
			for(int n = 0; n < network.inputLayer.nodes[i].weights.Length; n++)
			{				

				float c = network.inputLayer.nodes[i].weights[n];

				inputLayer[i].weights[n].color = new Color(0, c, c);
			}
		}
		for(int l = 0; l < hiddenLayers.Count; l++)
		{
			for(int n = 0; n < hiddenLayers[l].Count; n++)
			{
				for(int w = 0; w < network.hiddenLayers[l].nodes[n].weights.Length; w++)
				{
					float c = network.hiddenLayers[l].nodes[n].weights[w];

					hiddenLayers[l][n].weights[w].color = new Color(0, c, c);
				}
			}
		}
	}

	public void ObserveNetwork(NeuralNetwork network)
	{
		this.network = network;
		DrawNetwork();
	}

	public void ShutOff()
	{
		for(int i = 0; i < inputLayer.Count; i++)
		{
			Destroy(inputLayer[i].parent.gameObject);
		}
		for(int i = 0; i < hiddenLayers.Count; i++)
		{
			for(int n = 0; n < hiddenLayers[i].Count; n++)
			{
				Destroy(hiddenLayers[i][n].parent.gameObject);
			}
		}
		for(int i = 0; i < outputLayer.Count; i++)
		{
			Destroy(outputLayer[i].parent.gameObject);
		}
	}


	class NodeRenderer
	{

		public GameObject parent;
		public Image nodeImage;
		public List<Image> weights;
		public Text text;

		public NodeRenderer(Image nodeImage)
		{
			weights = new List<Image>();
			this.nodeImage = nodeImage;
		}
	}
}
