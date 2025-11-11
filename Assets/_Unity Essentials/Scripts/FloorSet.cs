using UnityEngine;

public class FloorSet : MonoBehaviour
{
    [SerializeField] private float minY = -4.38f;
    [SerializeField] private float targetY = 0.091f;
    [SerializeField] private float teleportCooldown = 0.2f; // tempo mínimo entre teleports (segundos)

    private Rigidbody rb3D;
    private Rigidbody2D rb2D;
    private float lastTeleportTime = -Mathf.Infinity;

    private void Awake()
    {
        rb3D = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Time.time - lastTeleportTime < teleportCooldown)
            return; // ainda no cooldown

        // Rigidbody 3D: altera apenas Y da posição e zera somente componente vertical da velocidade
        if (rb3D != null)
        {
            Vector3 pos = rb3D.position;
            if (pos.y <= minY)
            {
                pos.y = targetY;
                rb3D.position = pos;

                // preserva X/Z da velocidade e qualquer rotação, zera só Y para evitar teleports repetidos
                Vector3 v = rb3D.linearVelocity;
                rb3D.linearVelocity = new Vector3(v.x, 0f, v.z);

                lastTeleportTime = Time.time;
            }
            return;
        }

        // Rigidbody 2D: altera apenas Y da posição e zera somente componente vertical da velocidade
        if (rb2D != null)
        {
            Vector2 pos2 = rb2D.position;
            if (pos2.y <= minY)
            {
                pos2.y = targetY;
                rb2D.position = pos2;

                Vector2 v2 = rb2D.linearVelocity;
                rb2D.linearVelocity = new Vector2(v2.x, 0f);

                lastTeleportTime = Time.time;
            }
            return;
        }

        // Fallback sem Rigidbody: altera somente Y do transform e usa cooldown
        Vector3 posT = transform.position;
        if (posT.y <= minY)
        {
            posT.y = targetY;
            transform.position = posT;
            lastTeleportTime = Time.time;
        }
    }
}
