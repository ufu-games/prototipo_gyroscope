﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Player") {
			Debug.Log("Caiu no Buraco!");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}