using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public Material Material_jogador;

    private float attackTimer = 0f;
    private bool playerNaArea = false;
    public bool Inimigo_atacado = false;
    
    public Slider SliderVidaInimigo;
    public int vidaMaxima = 250;
    public int vidaAtual;
    public bool morto = false;
    private bool caindo = false;



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

        vidaAtual = vidaMaxima;

        // Configura o Slider
        SliderVidaInimigo.maxValue = vidaMaxima;
        SliderVidaInimigo.value = vidaAtual;




    }

    void Update()
    {
        SliderVidaInimigo.value = vidaAtual;
        if (player == null) return;
        if(vidaAtual <= 0){
            morto = true; caindo = true;
        }
        if (morto == false){
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
        } else if(morto == true && vidaAtual <= 0 && caindo == true)
        {
            morrer();
            caindo = false;
        }






        // contador de ataque
        if (playerNaArea && !Inimigo_atacado && !morto)
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
            Material_jogador.SetColor("_BaseColor", Color.red);
            Debug.Log("Inimigo atacou! Vida do player agora: " + playerScript.vida);
            StartCoroutine(Tempo_de_dano());

        }
    }
    private void morrer()
    {
        animator.SetTrigger("caiu");

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
        
        yield return new WaitForSeconds(0.5f);  // espera 1 segundos


        player_animator.SetBool("dano", false);
        Material_jogador.SetColor("_BaseColor", Color.white);
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
