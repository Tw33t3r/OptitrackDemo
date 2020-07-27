using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff_Fire : MonoBehaviour
{
    [SerializeField] private float swingVelocity = 1.0f;
    [SerializeField] private float swingDistance = 5.0f;

    private Transform transform_;
    private Vector3 lastPosition;
    private float deltaDistance;
    private float totalDistance = 0.0f;
    private float velocity;
    private ParticleSystem particle_fire;
    private ParticleSystem particle_charged;

    // Start is called before the first frame update
    void Start()
    {
        transform_ = this.gameObject.transform.GetChild(0);
        particle_fire = this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        particle_charged = this.gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        lastPosition = transform_.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (particle_fire.isPlaying)
            return;
        
        deltaDistance = Vector3.Distance(lastPosition, transform_.position);
        velocity = deltaDistance / Time.deltaTime;
        if (velocity > swingVelocity) // swing
        {
            totalDistance += deltaDistance;
            if (totalDistance > swingDistance) // charged
            {
                particle_charged.Play();
                var em = particle_charged.emission;
                em.rateOverTime = totalDistance * 10;
            }
        }
        else // holding
        {
            if (totalDistance > swingDistance) // fire
            {
                var em = particle_fire.main;
                em.duration = totalDistance / swingDistance / 2;
                particle_fire.Play();
            }
            totalDistance = 0;
            particle_charged.Stop();
        }

        lastPosition = transform_.position;
    }
}
