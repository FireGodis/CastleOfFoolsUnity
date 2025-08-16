using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 6f;
    public float jumpForce = 8f;
    public float dashSpeed = 14f;
    public float dashDuration = 0.15f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isDashing;
    private float dashTimer;

    private Animator animator; // <- para controlar as animações

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // pega o Animator do mesmo GameObject
    }

    void Update()
    {
        // Entrada WASD
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 inputMove = new Vector3(x, 0f, z).normalized;

        // movimento base (no plano XZ)
        Vector3 move = inputMove * speed;

        // === ANIMAÇÃO: está correndo ===
        bool estaCorrendo = inputMove.sqrMagnitude > 0.01f;
        animator.SetBool("estacorreno", estaCorrendo);

        // === PULO ===
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                velocity.y = jumpForce;
                animator.SetBool("estapulando", true);
            }
            else
            {
                if (velocity.y < 0f) velocity.y = -1f;
                animator.SetBool("estapulando", false);
            }
        }

        // gravidade
        velocity.y += Physics.gravity.y * Time.deltaTime;
        move.y = velocity.y;

        // === ATAQUE ===
        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("ataque");
        }

        // === DASH ===
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (inputMove.sqrMagnitude > 0.01f)
                move += inputMove * dashSpeed;
            else
                move += transform.forward * dashSpeed;

            if (dashTimer <= 0f) isDashing = false;
        }

        // move o personagem
        controller.Move(move * Time.deltaTime);
    }
}
