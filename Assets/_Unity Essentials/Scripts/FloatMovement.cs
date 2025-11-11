using UnityEngine;

public class FloatMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float floatSpeed = 1f;       // velocidade da flutuação (ciclos por segundo)
    public float floatAmplitude = 0.2f; // amplitude da flutuação (0.2 por padrão)

    public float scaleSpeed = 1f;       // velocidade do "piscar" (ciclos por segundo)
    public float scaleAmplitude = 0.1f; // amplitude do "piscar" (editável)

    private Vector3 startPos;
    private Vector3 startScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotaciona em Y (graus por segundo)
        transform.Rotate(0, rotationSpeed, 0);

        // Flutuação vertical usando seno
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed * Mathf.PI * 2f) * floatAmplitude;
        Vector3 pos = transform.localPosition;
        pos.y = newY;
        transform.localPosition = pos;

        // Piscar (diminuir/aumentar escala uniformemente) usando seno
        float scaleDelta = Mathf.Sin(Time.time * scaleSpeed * Mathf.PI * 2f) * scaleAmplitude;
        transform.localScale = startScale * (1f + scaleDelta);
    }
}
