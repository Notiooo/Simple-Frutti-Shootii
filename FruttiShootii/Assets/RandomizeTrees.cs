using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RandomizeTrees : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform[] treeTranforms = this.GetComponentsInChildren<Transform>();
        foreach(Transform treeTransform in treeTranforms) {
            if(treeTransform.gameObject == this.gameObject)
                continue;
            treeTransform.Rotate(0, Random.Range(0, 360), 0);
            treeTransform.localScale += new Vector3(0, Random.Range(-0.2f, 0.3f), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
