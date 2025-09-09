using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController3D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    public float vida = 100; // Valor de vida inicial

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded;
    private float lastGroundedTime;
    private float lastJumpPressedTime;

    private bool isAttacking = false;

    [Header("Referências")]
    public Animator animator;
    public Slider mana;
    public Slider SlidervVida;
    public ParticleSystem slash1;
    public ParticleSystem slash2;
    public ParticleSystem specialSlash;
    [SerializeField] private Transform spriteHolder;
    [SerializeField] private float flipOffset = 0.5f;
    private bool m_FacingRight = true;
    public GameObject inimigo; // Referência ao inimigo
    
    public Animator inimigo_animator_inimigo;
    public bool pode_atacar_inimigo = false;
    public GameObject Area_ataque;

    [Header("Efeito de Corrida")]
    public Transform pe; // ponto onde a partícula será criada
    public Transform pos_slash; // ponto onde a partícula de ataque será criada
    public ParticleSystem corridaParticlePrefab; // prefab do particle system
    private float lastParticleTime = 0f;
    public float particleInterval = 0.1f; // intervalo entre spawns

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        slash1.Stop();
        slash2.Stop();
        specialSlash.Stop();

        



        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteHolder == null && animator != null)
            spriteHolder = animator.transform;


    }

    void Update()
    {
        SlidervVida.value = vida/100;
        mana.value += Time.deltaTime * 0.1f; // Regenera mana lentamente
        // Se estiver atacando, trava o movimento e não processa entrada
        if (isAttacking)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!(stateInfo.IsName("Attack") || stateInfo.IsName("Attack2") || stateInfo.IsName("Special")))
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
            Debug.Log("valor de poder atacar é: " + pode_atacar_inimigo);
            StartAttack();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartAttack2();
        }

        if (Input.GetKeyDown(KeyCode.F) && mana.value == 1)
        {
            StartEspecial();
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
        
        slash1.Play();
        CancelMovement();
        animator.Play("Attack", 0, 0f);
        if (pode_atacar_inimigo && inimigo.GetComponent<EnemyScript>().morto == false)
        {
            
            inimigo_animator_inimigo .SetTrigger("dano");
            inimigo.GetComponent<EnemyScript>().vidaAtual -= 10;
            StartCoroutine(Tempo_de_dano_inimigo());
        }



    }
    private void StartAttack2()
    {
        isAttacking = true;
        slash2.Play();
        CancelMovement();
        animator.Play("Attack2", 0, 0f);
        if (pode_atacar_inimigo && inimigo.GetComponent<EnemyScript>().morto == false)
        {
            
            inimigo_animator_inimigo.SetTrigger("dano");
            inimigo.GetComponent<EnemyScript>().vidaAtual -= 30;
            StartCoroutine(Tempo_de_dano_inimigo());
        }
    }
    private void StartEspecial()
    {
        isAttacking = true;
        CancelMovement();
        ConsumirMana_Especial(1f); // Consome mana ao iniciar o ataque especial
        specialSlash.Play();
        mana.value = 0; // Consome mana ao iniciar o ataque especial
        animator.Play("Special", 0, 0f);
        if (pode_atacar_inimigo && inimigo.GetComponent<EnemyScript>().morto == false)
        {
            
            inimigo_animator_inimigo.SetTrigger("dano");
            inimigo.GetComponent<EnemyScript>().vidaAtual -= 30;
            StartCoroutine(Tempo_de_dano_inimigo_especial());
            


        }
    }

    private void ConsumirMana_Especial(float quantidade)
    {
        if (mana.value >= quantidade)
        {
            mana.value -= quantidade;
        }
        else
        {
            Debug.LogWarning("Mana insuficiente para realizar a ação.");
        }
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

    private IEnumerator Tempo_de_dano_inimigo()
    {
        
        yield return new WaitForSeconds(0.5f);  // espera 0.5 segundos

        

    }
    private IEnumerator Tempo_de_dano_inimigo_especial()
    {

        yield return new WaitForSeconds(0.8f);  // espera 1 segundos
        
        
           inimigo_animator_inimigo.SetTrigger("dano");
           inimigo.GetComponent<EnemyScript>().vidaAtual -= 50;
        
        

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
