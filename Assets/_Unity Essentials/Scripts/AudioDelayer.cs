using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDelayer : MonoBehaviour
{
    [Tooltip("Áudio reproduzido quando um objeto com tag 'Ball' colide com este objeto (tag 'Cube').")]
    public AudioClip clip;

    [Tooltip("Atraso em segundos antes de reproduzir o áudio.")]
    public float delay = 0f;

    [Tooltip("Exibe um aviso no Console se este GameObject não estiver com a tag 'Cube'.")]
    public bool warnIfNotCubeTag = true;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        audioSource.playOnAwake = false;

        if (warnIfNotCubeTag && !CompareTag("Cube"))
            Debug.LogWarning($"AudioDelayer: '{name}' não está com a tag 'Cube' (esperado).");

#if UNITY_2023_2_OR_NEWER
        if (UnityEngine.Object.FindFirstObjectByType<AudioListener>() == null)
            Debug.LogWarning("AudioDelayer: nenhum AudioListener encontrado na cena.");
#else
#pragma warning disable CS0618
        if (FindObjectOfType<AudioListener>() == null)
            Debug.LogWarning("AudioDelayer: nenhum AudioListener encontrado na cena.");
#pragma warning restore CS0618
#endif
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log($"{name}: OnCollisionEnter com '{other.gameObject.name}' (tag='{other.gameObject.tag}').");
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log($"{name}: Ball detectada em colisão — agendando áudio (delay={delay}s).");
            if (delay <= 0f) PlayClip();
            else StartCoroutine(PlayDelayed());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{name}: OnTriggerEnter com '{other.gameObject.name}' (tag='{other.gameObject.tag}').");
        if (other.CompareTag("Ball"))
        {
            Debug.Log($"{name}: Ball detectada em trigger — agendando áudio (delay={delay}s).");
            if (delay <= 0f) PlayClip();
            else StartCoroutine(PlayDelayed());
        }
    }

    IEnumerator PlayDelayed()
    {
        yield return new WaitForSeconds(delay);
        PlayClip();
    }

    void PlayClip()
    {   
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log($"{name}: PlayOneShot executado.");
        }
        else if (audioSource.clip != null)
        {
            audioSource.Play();
            Debug.Log($"{name}: Play do AudioSource.clip executado.");
        }
        else
        {
            Debug.LogWarning($"{name}: nenhum AudioClip atribuído (nem em 'clip' nem em AudioSource).");
        }
    }
}

