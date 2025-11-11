using UnityEngine;
using UnityEssentials;
using System.Collections.Generic;

public class CongratulationsScreen : MonoBehaviour
{
    [Header("Referências externas")]
    public TimeCounter timeCounter;
    public UpdateCollectibleCount collectibleCount;

    [Header("Objetos de UI a serem mostrados")]
    public List<GameObject> uiToShowList = new List<GameObject>();

    [Header("Opção de áudio")]
    public AudioClip audioClip;             // Som opcional a tocar
    public AudioSource audioSource;         // Fonte de áudio (pode ser o mesmo objeto ou outro)
    public bool playAudioOnce = true;       // Se deve tocar apenas uma vez

    private bool isShown = false;
    private bool audioPlayed = false;

    void Start()
    {
        // Garante que todos os objetos comecem desativados
        foreach (var uiObj in uiToShowList)
        {
            if (uiObj != null)
                uiObj.SetActive(false);
        }

        // Se o AudioSource não foi atribuído, tenta pegar automaticamente
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (timeCounter == null || collectibleCount == null || uiToShowList == null)
            return;

        // Condição: tempo maior que 0 e todos os colecionáveis foram coletados
        if (timeCounter.RemainingSeconds > 0f && collectibleCount.totalCollectibles == 0)
        {
            if (!isShown)
            {
                SetUIActive(true);
                PlayAudio();
                isShown = true;
            }
        }
        else if (isShown)
        {
            SetUIActive(false);
            isShown = false;
            audioPlayed = false; // reseta se quiser que possa tocar novamente
        }
    }

    private void SetUIActive(bool active)
    {
        foreach (var uiObj in uiToShowList)
        {
            if (uiObj != null)
                uiObj.SetActive(active);
        }
    }

    private void PlayAudio()
    {
        if (audioClip == null || audioSource == null)
            return;

        if (playAudioOnce && audioPlayed)
            return;

        audioSource.PlayOneShot(audioClip);
        audioPlayed = true;
    }
}
