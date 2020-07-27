using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private float swingVelocity = 1.0f;

    private Transform transform_;
    private Vector3 lastPosition;
    private float deltaDistance;
    private float velocity;
    private TrailRenderer trailRenderer;
    private GameObject hitBox;

    // Start is called before the first frame update
    void Start()
    {
        transform_ = this.gameObject.transform.GetChild(0);
        trailRenderer = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
        hitBox = this.gameObject.transform.GetChild(1).gameObject;
        lastPosition = transform_.position;
        hitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        deltaDistance = Vector3.Distance(lastPosition, transform_.position);
        velocity = deltaDistance / Time.deltaTime;
        if (velocity > swingVelocity) // swing
        {
            //trailRenderer.emitting = true;
            hitBox.SetActive(true);
        }
        else
        {
            //trailRenderer.emitting = false;
            hitBox.SetActive(false);
        }
        lastPosition = transform_.position;
    }
}
