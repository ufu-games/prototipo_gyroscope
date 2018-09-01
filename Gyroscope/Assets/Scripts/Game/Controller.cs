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
	private float m_maxBoosterVelocity = 10f;
	private bool m_isAccelerating = false;
	[SerializeField]
	private float m_turnAmount = 20f;

	private Rigidbody2D m_rigidbody;
	private AudioSource m_audioSource;
	private CameraFollow m_cameraFollow;

	void Start () {
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_audioSource = GetComponent<AudioSource>();
		m_cameraFollow = FindObjectOfType<CameraFollow>();

		m_audioSource.volume = 0;

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
		m_cameraFollow.UpdateOrtographicSize(m_currentVelocity, m_maxBoosterVelocity);
	}

	private IEnumerator Accelerate(float startSpeed, float aimSpeed, float timeToMove = 0.5f) {
		float elapsedTime = 0.0f;
		bool reachedVelocity = false;

		while(!reachedVelocity) {
			if(Mathf.Abs(aimSpeed - m_currentVelocity) < 0.1f) {
				reachedVelocity = true;
				m_currentVelocity = aimSpeed;
			}

			elapsedTime += Time.deltaTime;
			float t = Interpolation.EaseIn(Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f));
			m_currentVelocity = Mathf.Lerp(startSpeed, aimSpeed, t);

			yield return null;
		}

	}

	private IEnumerator IncreaseVolume(float start, float end) {
		m_audioSource.volume = start;
		float elapsedTime = 0.0f;
		bool reached = false;

		while(!reached) {
			if(Mathf.Abs(m_audioSource.volume - end) < 0.1f) {
				reached = true;
				m_audioSource.volume = end;
				break;
			}

			elapsedTime += Time.deltaTime;
			float t = Interpolation.EaseIn(Mathf.Clamp(elapsedTime, 0f, 1f));
			m_audioSource.volume = t;

			yield return null;
		}
	}

	public void StartMovement() {
		m_audioSource.Play();
		StartCoroutine(Accelerate(0f, m_maxVelocity));
		StartCoroutine(IncreaseVolume(0, 1f));
	}


	private IEnumerator BoostVelocityRoutine() {
		StartCoroutine(Accelerate(m_currentVelocity, m_maxBoosterVelocity, 0.5f));
		StartCoroutine(IncreaseVolume(1f, 1.5f));
		yield return new WaitForSeconds(1.5f);
		StartCoroutine(Accelerate(m_currentVelocity, m_maxVelocity, 1.5f));
		StartCoroutine(IncreaseVolume(1.5f, 1f));
	}


	private void BoostVelocity() {
		StopAllCoroutines();
		StartCoroutine(BoostVelocityRoutine());
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Booster") {
			BoostVelocity();
		}
	}
}
