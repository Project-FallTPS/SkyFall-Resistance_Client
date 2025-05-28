using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public ParticleSystem[] Particles;


    // Start is called before the first frame update
    void Start()
    {
        StopEmission();
    }

    private void Update()
    {
        if ((Input.GetKeyDown("h"))) //look around
        {
            StartEmission();
        }
    }

    public void StartEmission()
    {
        for (int i = 0; i < Particles.Length; i++)
        {
            Particles[i].Play();
        }
    }

   public void StopEmission()
    {
        for (int i = 0; i < Particles.Length; i++)
        {
            Particles[i].Stop();
        }
    }
}
