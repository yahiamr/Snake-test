﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void start() {
        Application.LoadLevel(1);

    }
    public void Quit()
    {
        Application.Quit();

    }
}
