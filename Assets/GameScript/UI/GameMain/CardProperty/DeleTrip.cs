using UnityEngine;
using System.Collections;

public class DeleTrip : MonoBehaviour {

    public float Time;
	void OnEnable () {
        Invoke("Dele", Time);
	}

    void Dele()
    {
        gameObject.SetActive(false);
    }
}
