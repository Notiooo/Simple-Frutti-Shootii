using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance;
    private AudioSource audioSource;
    private Queue<AudioClip> narrationQueue = new Queue<AudioClip>();
    private HashSet<AudioClip> playedClips = new HashSet<AudioClip>();

    private void Awake()
    {
        Debug.Log("Instance Awake: " + gameObject.name);

        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayNarration(AudioClip clip, bool shouldClearQueue = false, bool immediate = false)
    {
        if (shouldClearQueue)
        {
            narrationQueue.Clear();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        if (immediate || !audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
            playedClips.Add(clip);
        }
        else
        {
            narrationQueue.Enqueue(clip);
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying && narrationQueue.Count > 0)
        {
            AudioClip clip = narrationQueue.Dequeue();
            playedClips.Add(clip);
            audioSource.PlayOneShot(clip);
        }
    }

    public bool HasPlayed(AudioClip clip)
    {
        return playedClips.Contains(clip);
    }
}
