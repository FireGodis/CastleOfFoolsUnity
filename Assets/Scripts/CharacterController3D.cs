using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController3D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded;
    private float lastGroundedTime;
    private float lastJumpPressedTime;

    private bool isAttacking = false;

    [Header("Referências")]
    public Animator animator;
    [SerializeField] private Transform spriteHolder;
    [SerializeField] private float flipOffset = 0.5f;
    private bool m_FacingRight = true;

    [Header("Efeito de Corrida")]
    public Transform pe; // ponto onde a partícula será criada
    public ParticleSystem corridaParticlePrefab; // prefab do particle system
    private float lastParticleTime = 0f;
    public float particleInterval = 0.1f; // intervalo entre spawns

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteHolder == null && animator != null)
            spriteHolder = animator.transform;
    }

    void Update()
    {
        // Se estiver atacando, trava o movimento e não processa entrada
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Attack"))
            {
                isAttacking = false;
            }
            else
            {
                CancelMovement();
                return;
            }
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(moveX, 0f, moveZ).normalized * moveSpeed;

        if (isGrounded)
            lastGroundedTime = Time.time;

        if (Input.GetKeyDown(KeyCode.G))
            lastJumpPressedTime = Time.time;

        if (Time.time - lastJumpPressedTime <= jumpBufferTime &&
            Time.time - lastGroundedTime <= coyoteTime)
        {
            Jump();
            lastJumpPressedTime = -1;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartAttack();
        }

        bool correndo = moveInput.sqrMagnitude > 0.01f && isGrounded;
        animator.SetBool("estacorreno", correndo);

        // Criar partícula de corrida no intervalo definido
        if (correndo && Time.time - lastParticleTime >= particleInterval)
        {
            CriarParticulaCorrida();
            lastParticleTime = Time.time;
        }

        if (moveX > 0 && !m_FacingRight) Flip();
        else if (moveX < 0 && m_FacingRight) Flip();
    }

    private void CriarParticulaCorrida()
    {
        if (corridaParticlePrefab != null && pe != null)
        {
            ParticleSystem ps = Instantiate(corridaParticlePrefab, pe.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, 3f); // destruir após 3 segundos
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("estapulando");
        animator.SetBool("estanoar", true);
        isGrounded = false;
    }

    private void StartAttack()
    {
        isAttacking = true;
        CancelMovement();
        animator.Play("Attack", 0, 0f);
    }

    private void CancelMovement()
    {
        rb.linearVelocity = Vector3.zero;
        moveInput = Vector3.zero;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 localPos = spriteHolder.localPosition;
        Vector3 theScale = spriteHolder.localScale;
        theScale.x *= -1;
        spriteHolder.localScale = theScale;

        if (m_FacingRight)
            localPos.x = Mathf.Abs(localPos.x) - flipOffset;
        else
            localPos.x = -(Mathf.Abs(localPos.x) - flipOffset);

        spriteHolder.localPosition = localPos;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            isGrounded = true;
            animator.SetBool("estanoar", false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            isGrounded = false;
            animator.SetBool("estanoar", true);
        }
    }
}
