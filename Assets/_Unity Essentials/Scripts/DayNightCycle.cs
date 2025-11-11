using UnityEngine;

[AddComponentMenu("Unity Essentials/DayNightCycle")]
public class DayNightCycle : MonoBehaviour
{
    [Tooltip("Duração de um dia completo (360°) em segundos.")]
    public float dayDurationSeconds = 60f;

    [Tooltip("Posição inicial do dia como fração (0 = início, 0.5 = meio-dia).")]
    [Range(0f, 1f)]
    public float startTime = 0f;

    [Tooltip("Se verdadeiro, usa Time.unscaledDeltaTime (ignora Time.timeScale).")]
    public bool useUnscaledTime = false;

    // velocidade em graus por segundo
    private float degreesPerSecond;

    void Start()
    {
        if (dayDurationSeconds <= 0f)
            dayDurationSeconds = 1f; // proteção contra divisão por zero

        degreesPerSecond = 360f / dayDurationSeconds;

        // Define rotação inicial baseada em startTime (aplica no eixo local X)
        Vector3 e = transform.localEulerAngles;
        e.x = Mathf.Repeat(startTime * 360f, 360f);
        transform.localEulerAngles = e;
    }

    void Update()
    {
        float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        float deltaAngle = degreesPerSecond * dt;

        // Rotaciona ao redor do eixo X local (ajuste o eixo se necessário)
        transform.Rotate(Vector3.right, deltaAngle, Space.Self);
    }
}