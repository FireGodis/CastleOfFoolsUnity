using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f; // distância mínima do player
    public float attackDelay = 3f;   // tempo necessário dentro da área para atacar

    public Transform player;
    public Animator player_animator;
    private Rigidbody rb;
    public Animator animator; //animator do inimigo

    private float attackTimer = 0f;
    private bool playerNaArea = false;

    [Header("Referências")]
    [SerializeField] private Transform spriteHolder;
    [SerializeField] private float flipOffset = 0.5f;
    private bool m_FacingRight = true;

    [Header("Área de Ataque")]
    public Collider areaAtaque; // arraste o objeto AreaAtaque aqui

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // impede de girar sozinho
        



    }

    void Update()
    {
        
        if (player == null) return;

        // calcula direção até o player
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;
        animator.SetBool("estacorrendo", distance > attackRange);

        // se estiver longe o suficiente, anda em direção ao player
        if (distance > attackRange)
        {
            Vector3 moveDir = direction.normalized;
            rb.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);
        }

        // flip do sprite (baseado no X do player)
        if (direction.x > 0 && !m_FacingRight) Flip();
        else if (direction.x < 0 && m_FacingRight) Flip();

        // contador de ataque
        if (playerNaArea)
        {
            animator.SetBool("estacorrendo", false);
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDelay)
            {
                Atacar();
                attackTimer = 0f; // reseta após ataque
            }
        }
        else
        {
            attackTimer = 0f; // reset se player sair
        }
    }

    private void Atacar()
    {
        
        // acessa o script do jogador e tira vida
        CharacterController3D playerScript = player.GetComponent<CharacterController3D>();
        if (playerScript != null)
        {
            playerScript.vida -= 10;
            animator.SetTrigger("attack");
            player_animator.SetBool("dano", true);
            Debug.Log("Inimigo atacou! Vida do player agora: " + playerScript.vida);
            StartCoroutine(Tempo_de_dano());

        }
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

    private IEnumerator Tempo_de_dano()
    {

        yield return new WaitForSeconds(1f);  // espera 1 segundos


        player_animator.SetBool("dano", false);
    }

    // detecta se o player entrou na área de ataque
    private void OnTriggerEnter(Collider other)
    {
        
        
        
        if (other.CompareTag("Player"))
        {
            
            playerNaArea = true;
            attackTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            playerNaArea = false;
            attackTimer = 0f;
        }
    }
}
