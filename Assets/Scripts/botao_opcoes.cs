using UnityEngine;

public class botao_opcoes : MonoBehaviour
{

    public GameObject logo_neon;
    public GameObject logo_opcoes;
    public GameObject play;
    public GameObject opcoes;
    public GameObject sair;
    public GameObject voltar;
    public GameObject botaoplay;
    public GameObject botaoopcoes;
    public GameObject botaosair;
    public GameObject botaovoltar;
    public GameObject botaodublagem;

    public bool dublagem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dublagem = true;
}

    public void abrir_opcoes()
    {
        logo_neon.SetActive(false);
        play.SetActive(false);
        opcoes.SetActive(false);
        sair.SetActive(false);
        voltar.SetActive(true);
        botaoplay.SetActive(false);
        botaoopcoes.SetActive(false);
        botaosair.SetActive(false);

        logo_opcoes.SetActive(true);
        voltar.SetActive(true);
        botaovoltar.SetActive(true);
        botaodublagem.SetActive(true);
    }

    public void toggle_dublagem()
    {
        dublagem = !dublagem;
        Debug.Log("Dublagem: " + dublagem);
    }

    public void fechar_opcoes()
    {
        logo_neon.SetActive(true);
        play.SetActive(true);
        opcoes.SetActive(true);
        sair.SetActive(true);
        voltar.SetActive(false);
        botaoplay.SetActive(true);
        botaoopcoes.SetActive(true);
        botaosair.SetActive(true);
        logo_opcoes.SetActive(false);
        voltar.SetActive(false);
        botaovoltar.SetActive(false);
        botaodublagem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
