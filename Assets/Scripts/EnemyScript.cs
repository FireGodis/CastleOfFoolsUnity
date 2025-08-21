using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f; // distância mínima do player
    public float attackDelay = 3f;   // tempo necessário dentro da área para atacar

    private Transform player;
    private Rigidbody rb;

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

        // acha o player pela tag
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer != null)
            player = objPlayer.transform;
    }

    void Update()
    {
        if (player == null) return;

        // calcula direção até o player
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

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
            Debug.Log("Inimigo atacou! Vida do player agora: " + playerScript.vida);
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

    // detecta se o player entrou na área de ataque
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaArea = true;
            attackTimer = 0f;
        }
    }

    // detecta se o player saiu da área de ataque
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNaArea = false;
            attackTimer = 0f;
        }
    }
}
