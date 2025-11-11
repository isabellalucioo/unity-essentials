using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject onCollectEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);

    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")){
            //instanciar o efeito de coleta na posição e rotação do coletável
            Instantiate(onCollectEffect, transform.position, transform.rotation);

            //tocar áudio de coleta (se houver) - usa PlayClipAtPoint para que o som continue mesmo após destruir o objeto
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null) {
                if (audioSource.clip != null) {
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, audioSource.volume);
                } else {
                    audioSource.Play(); // fallback caso o AudioSource tenha um AudioSource configurado sem clip (raro)
                }
            }

            //destruir o coletável quando o jogador tocar
            Destroy(gameObject);
        }

    }

}
