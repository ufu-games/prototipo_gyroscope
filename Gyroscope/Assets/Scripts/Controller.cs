using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	[Header("Variáveis para Debug")]
	public Text debugText;

	[Header("Variáveis de Controle")]
	[SerializeField]
	private float m_maxVelocity = 5f;
	[SerializeField]
	private float m_turnAmount = 20f;

	private Rigidbody2D m_rigidbody;

	void Start () {
		m_rigidbody = GetComponent<Rigidbody2D>();

		#if UNITY_ANDROID
		Input.gyro.enabled = true;
		#endif	
	}
	
	
	void Update () {
		float angleZ = transform.localEulerAngles.z;

		if(!Input.gyro.enabled) {
			if(Input.GetKey(KeyCode.RightArrow)) {
				angleZ -= (m_turnAmount + Time.deltaTime);
			} else if(Input.GetKey(KeyCode.LeftArrow)) {
				angleZ += (m_turnAmount + Time.deltaTime);
			}
		} else {
			float rotation = Input.gyro.rotationRateUnbiased.z;
			if(debugText) debugText.text = "rotation: " + Mathf.Round(rotation * 100);
			if(Mathf.Abs(rotation) < 0.1f) rotation = 0;

			angleZ += (rotation * m_turnAmount * Time.deltaTime);
		}

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angleZ);
		m_rigidbody.velocity = transform.up * m_maxVelocity;
	}
}
