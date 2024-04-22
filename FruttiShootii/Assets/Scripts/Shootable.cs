using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEditor.AI;

public class Shootable : MonoBehaviour
{
    Rigidbody rigidBody;
    Animator animator;
    AudioSource audioSource;

    ParticleSystem particleSystem;

    UnityEngine.AI.NavMeshAgent navMeshAgent;

    public float health = 1;
    bool alive = true;
    public Transform player;
    public bool debugMode = false;

    private float forceMultiplayer = 1000f;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponentInChildren<AudioSource>();
        particleSystem = this.GetComponentInChildren<ParticleSystem>();
        navMeshAgent = this.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        KillMeTest();
    }

    public void Hit(float damage, Vector3 hitForce, Vector3 hitPosition) {
        Debug.Log("HIT!");
        health -= damage;
        if(health < 0) {
            alive = false;
        }

        // Play sound
        if(audioSource != null) {
            audioSource.Play();
        }

        // Play particle effect
        PlayParticleSystem(hitForce, hitPosition);

        AnimateOnHit();
        ApplyForce(hitForce, hitPosition);

    }

    void PlayParticleSystem(Vector3 hitForce, Vector3 hitPosition) {
        if(particleSystem != null) {
            particleSystem.Clear();
            particleSystem.transform.rotation = Quaternion.LookRotation(-hitForce);
            particleSystem.transform.position = hitPosition;
            particleSystem.Play();
        }
    }

    void ApplyForce(Vector3 hitForce, Vector3 hitPosition) {
        if(!alive) {
            if (rigidBody != null) {
                rigidBody.freezeRotation = false; 
                rigidBody.isKinematic = false;
                rigidBody.AddForceAtPosition((hitForce.normalized) * forceMultiplayer, hitPosition);
            }
        }
    }

    void AnimateOnHit() {
        if (!alive) {
            if (animator != null) {
                animator.enabled = false;
            }

            if(navMeshAgent != null){
                navMeshAgent.enabled = false;
            }
            //this.GetComponentInChildren<PlantTransmutator>()?.Tansmutate();
        }
    }

    public bool IsAlive() {
        return alive;
    }

    private void KillMeTest(){
        if (debugMode && Input.GetKeyDown(KeyCode.P))
        {
            Hit(100f, transform.position - player.position, transform.position + new Vector3(0f, 0.5f, 0f));
        }
    }
}
