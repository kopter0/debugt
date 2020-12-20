using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingParticlesScript : MonoBehaviour
{

    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        //Collider[] npcs = GameObject.Find("ActiveNPCFolder").GetComponentsInChildren<Collider>();
        //for (int i = 0; i < npcs.Length; i++)
        //{
        //    ps.trigger.SetCollider(i, npcs[i]);
        //}

    }

    //private void OnDisable()
    //{
    //    ps.trigger.
    //}
}
