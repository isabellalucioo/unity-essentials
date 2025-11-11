using System.Collections.Generic;
using UnityEngine;

// Controls player movement and rotation.
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // Set player's movement speed.
    public float rotationSpeed = 120.0f; // Set player's rotation speed.

    private Rigidbody rb; // Reference to player's Rigidbody.
    public float jumpForce = 5.0f; // Set player's jump force.

    // Limite de pulos: máximo de 2 pulos por janela de 2 segundos.
    public int maxJumpsPerWindow = 2;
    public float jumpWindow = 2f; // segundos
    private List<float> jumpTimestamps = new List<float>();

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Access player's Rigidbody.
        // Constrain rotation to Y axis only (bloqueia X e Z)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CleanOldJumps();

            if (jumpTimestamps.Count < maxJumpsPerWindow)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                jumpTimestamps.Add(Time.time);
            }
            // Caso ultrapasse o limite, ignora o input (pode adicionar feedback aqui se quiser)
        }
    }

    // Remove timestamps fora da janela de tempo.
    private void CleanOldJumps()
    {
        float cutoff = Time.time - jumpWindow;
        jumpTimestamps.RemoveAll(t => t < cutoff);
    }

    // Handle physics-based movement and rotation.
    private void FixedUpdate()
    {
        // Move player based on vertical input.
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate player based on horizontal input.
        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}

