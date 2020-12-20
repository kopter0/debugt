using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggerScript : MonoBehaviour
{

    private PlayerMechanicsScript pms;
    private ParticleSystem ps;

    private void Start()
    {
        pms = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMechanicsScript>();
        ps = transform.GetComponent<ParticleSystem>();
    }
    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int n = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        pms.UpdateStressLevel(n * 1.0f);
    }
}
