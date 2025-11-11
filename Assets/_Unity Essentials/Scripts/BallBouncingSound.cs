using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallBouncingSound : MonoBehaviour
{
    [Tooltip("Posição Y alvo que dispara o áudio")]
    public float targetY = -1.671421f;

    [Tooltip("Tolerância para detecção (em unidades Unity)")]
    public float tolerance = 0.001f;

    [Tooltip("Áudio a ser reproduzido (opcional se o AudioSource já tiver um clip)")]
    public AudioClip clip;

    [Tooltip("Se verdadeiro, permite que o som seja tocado novamente ao atravessar a posição novamente")]
    public bool allowRepeat = false;

    private AudioSource audioSource;
    private float previousY;
    private bool played;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource.clip == null && clip != null)
            audioSource.clip = clip;

        previousY = transform.position.y;
        played = false;
    }

    void Update()
    {
        float currentY = transform.position.y;

        // Detecta atravessamento da posição alvo (cima->baixo ou baixo->cima)
        bool crossed = (previousY > targetY && currentY <= targetY) || (previousY < targetY && currentY >= targetY);
        // Detecta presença dentro da tolerância
        bool withinTolerance = Mathf.Abs(currentY - targetY) <= tolerance;

        if (!played)
        {
            if (crossed || withinTolerance)
                PlaySound();
        }
        else if (allowRepeat)
        {
            // Reseta o trigger quando sair da faixa de tolerância para permitir novo disparo
            if (Mathf.Abs(currentY - targetY) > tolerance)
                played = false;
        }

        previousY = currentY;
    }

    private void PlaySound()
    {
        if (audioSource == null)
            return;

        if (clip != null)
            audioSource.PlayOneShot(clip);
        else
            audioSource.Play();

        played = true;
    }
}
