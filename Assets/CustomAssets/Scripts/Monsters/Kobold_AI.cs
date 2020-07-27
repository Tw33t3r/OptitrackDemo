using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (MonsterMovement))]
    public class Kobold_AI : MonoBehaviour
    {
        [SerializeField] private float health = 100.0f;
        [SerializeField] private float mass = 50.0f;
        [SerializeField] private float attackDistance = 3.0f;
        [SerializeField] private float retreatDistance = 2.5f;
        [SerializeField] private float attackCoolTime = 2.0f;
        [SerializeField] private float hitFreezingTime = 1.0f;
        [SerializeField] private float deadRemainingTime = 10.0f;

        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public MonsterMovement character { get; private set; } // the character we are controlling

        private Transform target;                                    // target to aim for
        private Animator animator;
        private GameObject damageBox;
        private bool bNowAttack = false;
        private bool bApproach = true;
        private bool bRetreatSetted = false;
        private float timeCounter = 0;

        private List<ParticleCollisionEvent> collisionEvents;
        private Vector3 impact = Vector3.zero;
        private float freezingCounter = 0;
        bool bHit = false;
        bool bIdle = true;
        bool bDeath = false;


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<MonsterMovement>();
            animator = GetComponent<Animator>();
            collisionEvents = new List<ParticleCollisionEvent>();
            damageBox = this.gameObject.transform.Find("root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/WeaponSlot/DamageBox").gameObject;
            target = GameObject.Find("/[CameraRig]/Camera").transform;

            damageBox.SetActive(false);
            agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            if (bHit)
            {
                agent.isStopped = true;
                freezingCounter = 0;
                bHit = false;

                if (health <= 0) // die
                {
                    animator.SetBool("Death", true);
                    Destroy(this.gameObject, deadRemainingTime);
                    bDeath = true;
                }
                else // hit
                    animator.SetTrigger("Hit");
            }
            else if (bIdle)
            {
                if (bApproach)
                {
                    if (target != null)
                        agent.SetDestination(target.position);

                    if (agent.remainingDistance > attackDistance)  // move forward
                        character.Move(agent.desiredVelocity, false, false);
                    else if (agent.remainingDistance > retreatDistance && !bNowAttack)  // attack
                    {
                        // agent.isStopped = true;
                        animator.SetTrigger("Attack");
                        damageBox.SetActive(true);
                        bNowAttack = true;
                    }
                    else if (agent.remainingDistance < retreatDistance && !bNowAttack)  // retreat
                        bApproach = false;

                    if (bNowAttack)
                    {
                        if (timeCounter > attackCoolTime)
                        {
                            timeCounter = 0;
                            bNowAttack = false;
                            damageBox.SetActive(false);
                            // agent.isStopped = false;
                        }
                        else
                            timeCounter += Time.deltaTime;
                    }
                }
                else
                {
                    if (target != null && !bRetreatSetted)
                    {
                        Vector3 retreatTarget = new Vector3(target.position.x - 5.0f, target.position.y, target.position.z);
                        retreatTarget = Quaternion.Euler(new Vector3(0.0f, UnityEngine.Random.Range(-30.0f, 30.0f), 0.0f)) * (retreatTarget - target.position) + target.position;
                        agent.SetDestination(retreatTarget);
                        bRetreatSetted = true;
                    }
                    if (agent.remainingDistance > attackDistance)  // retreat
                        character.Move(agent.desiredVelocity, false, false);
                    else if (agent.remainingDistance < 1)  // Approach again
                    {
                        character.Move(Vector3.zero, false, false);
                        bApproach = true;
                        bRetreatSetted = false;
                    }
                }
            }
            else
            {
                freezingCounter += Time.deltaTime;

                // apply the impact force:
                //if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime, false, false);
                if (impact.magnitude > 0.2) gameObject.transform.position = gameObject.transform.position + (impact * Time.deltaTime);
                // consumes the impact energy each cycle:
                impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

                if (!bDeath)
                {
                    if (freezingCounter > hitFreezingTime)
                    {
                        bIdle = true;
                        agent.isStopped = false;
                    }
                }
            }
        }

        private void OnParticleCollision(GameObject particleObj)
        {
            ParticleSystem particle = particleObj.GetComponent<ParticleSystem>();
            int numCollisionEvents = particle.GetCollisionEvents(this.gameObject, collisionEvents);
            Vector3 colPos = collisionEvents[0].intersection;
            bIdle = false;
            bHit = true;

            if (particle.name == "fireShot")
            {
                health -= 30;
                var dir = this.gameObject.transform.position - colPos;
                var force = 300.0f;
                AddImpact(dir, force);
            }
            else if(particle.name == "LightningShot")
            {
                health -= 60;
                var dir = this.gameObject.transform.position - colPos;
                var force = 3000.0f;
                AddImpact(dir, force);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            //Vector3 colPos = otherObj.GetContact(0).normal;
            if (collision.gameObject.name == "HitBox") // Hammer hitted
            {
                bIdle = false;
                bHit = true;
                health -= 30;
                var dir = this.gameObject.transform.position - collision.transform.position;
                var force = 1500.0f;
                AddImpact(dir, force);
            }
        }

        private void AddImpact(Vector3 dir, float force)
        {
            dir.Normalize();
            if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
            impact += dir.normalized * force / mass;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}