using UnityEngine;
using System.Collections;

public class GlobalGameController : MonoBehaviour {
    
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
