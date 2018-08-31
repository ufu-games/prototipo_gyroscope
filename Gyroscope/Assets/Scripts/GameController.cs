using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject beginningScreen;
	private Controller m_carController;

	void Awake() {
		m_carController = FindObjectOfType<Controller>();
		beginningScreen.SetActive(true);
	}
	public void StartGame() {
		beginningScreen.SetActive(false);
		m_carController.StartMovement();
		Debug.Log("Start Game!");
	}
}
