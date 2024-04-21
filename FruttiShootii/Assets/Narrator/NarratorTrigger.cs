using System.Collections;
using UnityEngine;

[System.Serializable]
public struct NarrationClip
{
    public AudioClip clip;
    public float delay;
    public bool isUrgent;
    public bool shouldClearQueue;
}

public class NarratorTrigger : MonoBehaviour
{
    public NarrationClip[] narrationClips;
    private bool hasBeenTriggered = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            StartCoroutine(PlayNarrationSequence());
        }
    }

    IEnumerator PlayNarrationSequence()
    {
        foreach (var narrationClip in narrationClips)
        {
            yield return new WaitForSeconds(narrationClip.delay); 
            NarrationManager.Instance.PlayNarration(narrationClip.clip, narrationClip.shouldClearQueue, narrationClip.isUrgent);
            yield return new WaitForSeconds(narrationClip.clip.length);
        }
    }
}
