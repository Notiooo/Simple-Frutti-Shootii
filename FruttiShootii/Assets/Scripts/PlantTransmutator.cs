using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTransmutator : MonoBehaviour
{
    public GameObject plantMesh;
    public GameObject humanMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tansmutate() {
        plantMesh.SetActive(false);
        humanMesh.SetActive(true);
    }
}
