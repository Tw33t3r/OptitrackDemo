using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    [SerializeField] private float swingVelocity = 1;
    [SerializeField] private float swingDistance = 3;

    private Transform transform_;
    private Vector3 lastPosition;
    private float deltaDistance;
    private float totalDistance = 0;
    private float velocity;
    private ParticleSystem particle_fire;

    // Start is called before the first frame update
    void Start()
    {
        transform_ = this.gameObject.transform.GetChild(0);
        particle_fire = this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        lastPosition = transform_.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (particle_fire.isPlaying)
            return;
        
        deltaDistance = Vector3.Distance(lastPosition, transform_.position);
        velocity = deltaDistance / Time.deltaTime;
        if(velocity > swingVelocity) // swing
            totalDistance += deltaDistance;
        else // holding
        {
            if (totalDistance > swingDistance) // fire
            {
                particle_fire.Play();
            }
            totalDistance = 0;
        }

        lastPosition = transform_.position;
    }
}
