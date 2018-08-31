using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Interpolation {
	
	// EaseIn => O início é lento
	public static float EaseIn(float v) {
		return (1 - Mathf.Cos(v * Mathf.PI * 0.5f));
	}

	// Ease Out => O início é rapido
	public static float EaseOut(float v) {
		return (Mathf.Sin(v * Mathf.PI * 0.5f));
	}

	public static float SmoothStep(float v) {
		return (v*v*(3 - 2*v));
	}

	public static float SmootherStep(float v) {
		return (v*v*v*(v*(v*6 - 15) + 10));
	}
}