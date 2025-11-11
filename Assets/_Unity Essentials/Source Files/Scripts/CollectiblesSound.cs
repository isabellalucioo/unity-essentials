using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectiblesSound : MonoBehaviour
{
    [Tooltip("Tag do objeto que dispara o som (ex: \"Player\"). Deixe vazio para aceitar qualquer collider).")]
    public string triggerTag = "Player";

    [Tooltip("Se verdadeiro, toca o som automaticamente ao instanciar.")]
    public bool playOnInstantiate = false;

    [Tooltip("Se verdadeiro, destrói o GameObject depois do som tocar.")]
    public bool destroyAfterPlay = true;

    [Tooltip("Clip alternativo caso o AudioSource do prefab não tenha um AudioClip.")]
    public AudioClip fallbackClip;

    AudioSource audioSource;
    bool played = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log($"[CollectiblesSound] Awake: '{name}' AudioSource={(audioSource!=null)} clip={(audioSource!=null && audioSource.clip!=null ? audioSource.clip.name : "null")} fallback={(fallbackClip!=null ? fallbackClip.name : "null")}");
    }

    void Start()
    {
        if (playOnInstantiate)
        {
            PlayAndMaybeDestroy();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[CollectiblesSound] OnTriggerEnter2D: '{name}' collided with '{other.name}' tag='{other.tag}'");
        if (played) return;
        if (!string.IsNullOrEmpty(triggerTag) && !other.CompareTag(triggerTag)) return;
        PlayAndMaybeDestroy();
    }

    void PlayAndMaybeDestroy()
    {
        if (played) return;
        if (audioSource == null)
        {
            Debug.LogWarning($"[CollectiblesSound] Sem AudioSource em '{name}'.");
            if (destroyAfterPlay) Destroy(gameObject);
            played = true;
            return;
        }

        // Garantir que o componente está habilitado (resolve o erro que você viu)
        if (!audioSource.enabled)
        {
            Debug.LogWarning($"[CollectiblesSound] AudioSource estava desabilitado em '{name}'; habilitando antes de tocar.");
            audioSource.enabled = true;
        }

        AudioClip clipToPlay = audioSource.clip != null ? audioSource.clip : fallbackClip;
        if (clipToPlay == null)
        {
            Debug.LogWarning($"[CollectiblesSound] Nenhum AudioClip atribuído em '{name}'. Nada para tocar.");
            if (destroyAfterPlay) Destroy(gameObject);
            played = true;
            return;
        }

        // Se o GameObject do AudioSource estiver inativo, PlayOneShot falhará; usar fallback
        if (!audioSource.gameObject.activeInHierarchy)
        {
            Debug.LogWarning($"[CollectiblesSound] GameObject do AudioSource está inativo em '{name}'. Usando PlayClipAtPoint como fallback.");
            AudioSource.PlayClipAtPoint(clipToPlay, transform.position);
            played = true;
            if (destroyAfterPlay) Destroy(gameObject, clipToPlay.length + 0.05f);
            return;
        }

        audioSource.PlayOneShot(clipToPlay);
        played = true;

        if (destroyAfterPlay)
        {
            Destroy(gameObject, clipToPlay.length + 0.05f);
        }
    }
}
