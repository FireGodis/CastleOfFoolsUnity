using UnityEngine;

public class AreaAtaque : MonoBehaviour
{
    public CharacterController3D player; // referência ao Player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inimigo"))
        {
            player.pode_atacar_inimigo = true;
            Debug.Log("Inimigo entrou na área de ataque");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inimigo"))
        {
            player.pode_atacar_inimigo = false;
            Debug.Log("Inimigo saiu da área de ataque");
        }
    }
}
