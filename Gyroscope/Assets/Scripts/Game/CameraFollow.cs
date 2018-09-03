using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private float m_originalOrtographicSize = 3f;
	private float m_maxOrtographicSize = 5f;

	/*
		Ideia:
		O tamanho original da camera é 5, a ideia é aumentar para 7 na velocidade máxima (quando passar por booster) e aumentar para 6 (ou algo em torno disso) na aceleração normal)
	*/

	public void UpdateOrtographicSize(float carVelocity, float maxVelocity) {
		float variableAmount = m_maxOrtographicSize - m_originalOrtographicSize;
		float toAdd = variableAmount * (carVelocity / maxVelocity);

		Camera.main.orthographicSize = (m_originalOrtographicSize + toAdd);
	}
}
