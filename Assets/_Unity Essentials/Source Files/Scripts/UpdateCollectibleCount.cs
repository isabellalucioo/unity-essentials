using UnityEngine;
using TMPro;
using System; // Required for Type handling

namespace UnityEssentials
{
    public class UpdateCollectibleCount : MonoBehaviour
    {
        private TextMeshProUGUI collectibleText; // Reference to the TextMeshProUGUI component

        [SerializeField] private AudioSource audioSource; // assign in Inspector or it will try GetComponent
        [SerializeField] private AudioClip victoryClip;   // optional: played with PlayOneShot if assigned
        [SerializeField] private bool playOnce = true;    // set false to allow replay when entering zero repeatedly

        private bool hasPlayed = false;

        // 🔹 Agora é pública, assim outros scripts (como CongratulateScreen) podem ler:
        [HideInInspector] public int totalCollectibles = 0;

        void Start()
        {
            collectibleText = GetComponent<TextMeshProUGUI>();
            if (collectibleText == null)
            {
                Debug.LogError("UpdateCollectibleCount script requires a TextMeshProUGUI component on the same GameObject.");
                return;
            }

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null && victoryClip != null)
                {
                    // If no AudioSource but a clip was provided, add a temporary AudioSource
                    audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                }
            }

            UpdateCollectibleDisplay(); // Initial update on start
        }

        void Update()
        {
            UpdateCollectibleDisplay();
        }

        private void UpdateCollectibleDisplay()
        {
            totalCollectibles = 0;

            // Check and count objects of type Collectible
            Type collectibleType = Type.GetType("Collectible");
            if (collectibleType != null)
            {
                totalCollectibles += UnityEngine.Object.FindObjectsByType(collectibleType, FindObjectsSortMode.None).Length;
            }

            // Optionally, check and count objects of type Collectible2D as well if needed
            Type collectible2DType = Type.GetType("Collectible2D");
            if (collectible2DType != null)
            {
                totalCollectibles += UnityEngine.Object.FindObjectsByType(collectible2DType, FindObjectsSortMode.None).Length;
            }

            // Update the collectible count display; show congratulatory message when zero
            if (totalCollectibles == 0)
            {
                collectibleText.text = "Congrats! You got them all!";

                if (!hasPlayed)
                {
                    if (audioSource != null)
                    {
                        if (victoryClip != null)
                            audioSource.PlayOneShot(victoryClip);
                        else
                            audioSource.Play();

                        if (playOnce) hasPlayed = true;
                    }
                    else
                    {
                        Debug.LogWarning("No AudioSource available to play victory audio. Assign an AudioSource or an AudioClip in the Inspector.");
                    }
                }
            }
            else
            {
                collectibleText.text = $"Collectibles remaining: {totalCollectibles}";
                if (!playOnce)
                {
                    // allow replay if playOnce == false and collectibles go above 0 then back to 0
                    hasPlayed = false;
                }
            }
        }
    }
}
