using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotionPour : MonoBehaviour
{
    Transform flaskTip;
    public AudioClip audioClip;
    bool successfullPour = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++){
            if(this.transform.GetChild(i).name == "flaskUp")
                flaskTip = this.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PourOut() {
        RaycastHit rayHit;
        Physics.Raycast(flaskTip.position, Vector3.down, out rayHit, 10);
        if(rayHit.transform.name == "WateringCan" && !successfullPour){
            successfullPour = true;
            NarrationManager.Instance.PlayNarration(audioClip, false, true);
        }
    }
}
