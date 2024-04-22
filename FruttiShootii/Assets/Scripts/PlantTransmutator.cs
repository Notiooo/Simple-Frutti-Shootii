using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTransmutator : MonoBehaviour
{

    public GameObject plantMesh;
    public GameObject humanMesh;
    
    int stage = 0;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.speed = 0f;
        plantMesh.SetActive(true);
        humanMesh.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tansmutate() {
        if(stage == 0){
            animator.speed = 1f;
            stage = 1;
        } else if(stage == 1){
            plantMesh.SetActive(false);
            humanMesh.SetActive(true);
        }

        
        
    }
}
