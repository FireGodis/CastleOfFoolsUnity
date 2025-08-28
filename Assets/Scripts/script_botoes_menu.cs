using UnityEngine;

public class script_botoes_menu : MonoBehaviour
{
    public Canvas CanvasPlayer;
    public Canvas Tela_menu;
    public GameObject Player;
    
    public GameObject Inimigo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jogar()
    {
        CanvasPlayer.gameObject.SetActive(true);
        Player.SetActive(true);
        
        Inimigo.SetActive(true);
        Tela_menu.gameObject.SetActive(false);
    }
}
