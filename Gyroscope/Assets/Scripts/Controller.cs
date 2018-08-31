using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	[Header("Variáveis para Debug")]
	public Text debugText;
	public Text velocityText;

	[Header("Variáveis de Controle")]
	[SerializeField]
	private float m_currentVelocity = 0f;
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

		if(velocityText) velocityText.text = ("velocidade: " + m_currentVelocity);

		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angleZ);
		m_rigidbody.velocity = transform.up * m_currentVelocity;
	}

	private IEnumerator Accelerate(float startSpeed, float aimSpeed, float timeToMove = 0.2f) {
		float elapsedTime = 0.0f;
		bool reachedVelocity = false;

		while(!reachedVelocity) {
			if(aimSpeed - m_currentVelocity < 0.1f) {
				reachedVelocity = true;
				m_currentVelocity = aimSpeed;
			}

			elapsedTime += Time.deltaTime;
			float t = Interpolation.EaseIn(Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f));
			m_currentVelocity = Mathf.Lerp(startSpeed, aimSpeed, t);

			yield return null;
		}
	}

	public void StartMovement() {
		StartCoroutine(Accelerate(0f, m_maxVelocity));
	}
}
