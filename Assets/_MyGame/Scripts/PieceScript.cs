using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PieceScript : MonoBehaviour
{
    // PieceScript will take over control of player during this piece
    [SerializeField]
    private GameObject player;

    // factor for player horizontal speed
    [SerializeField]
    private float beatSpeed = 1.0f;

    [Header("Music Settings")]
    // time signature of the music pice
    [SerializeField]
    private int beatPerBar = 4;

    // tempo i.e. bpm of the music
    [SerializeField]
    private float tempo = 120.0f;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set up audio source  ------------------------------------------------
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update() { }
}


// Fixme map need to distinguish b/t purposes of dash vs jump,
// also allow different actions for the same action
