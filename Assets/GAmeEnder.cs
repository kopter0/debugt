using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAmeEnder : MonoBehaviour
{

    private BossScript bs;
    public GameObject prompt;
    private GameControllerScript gcs;

    private void Start()
    {
        gcs = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }


    private void OnTriggerEnter(Collider other)
    {
        prompt.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            bs = GameObject.FindGameObjectWithTag("boss").GetComponentInChildren<BossScript>();
            bs.Revive();
            prompt.SetActive(false);
            gcs.GameEnded(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        prompt.SetActive(false);
    }
}
