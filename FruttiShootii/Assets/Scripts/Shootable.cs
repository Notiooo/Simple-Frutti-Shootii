using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    Rigidbody rigidBody;
    Animator animator;
    AudioSource audioSource;

    ParticleSystem particleSystem;

    public float health = 1;
    bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        audioSource = this.GetComponentInChildren<AudioSource>();
        particleSystem = this.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                rigidBody.isKinematic = false;
                rigidBody.AddForceAtPosition(hitForce, hitPosition);
            }
        }
    }

    void AnimateOnHit() {
        if (!alive) {
            if (animator != null) {
                animator.enabled = false;
            }
            //this.GetComponentInChildren<PlantTransmutator>()?.Tansmutate();
        }
    }

    public bool IsAlive() {
        return alive;
    }
}
