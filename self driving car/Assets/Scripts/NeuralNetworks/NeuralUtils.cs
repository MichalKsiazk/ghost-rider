using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeuralUtils {

	public static float Sigmoid(float x)
	{
		float output = 1 / (1 + Mathf.Exp(-x));
		return output;
	}

	public static float CustomSigmoid(float x, float y_shift, float y_range, float multiplier)
	{
		float output = ((1 / (1 + Mathf.Exp(-x * multiplier))) + y_shift) * y_range;
		return output;
	}

}
