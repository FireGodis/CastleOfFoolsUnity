using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;                // arraste aqui seu asset DialogueData (ScriptableObject)
    public DialogueSystem dialogueSystem;        // arraste aqui o GameObject que possui DialogueSystem
    public bool passar_mais_de_uma_vez = false; // se pode passar mais de uma vez

    private bool ja_passou = false;

    public void TriggerDialogue()
    {
        DialogueSystem ds = dialogueSystem;

        // se não tiver arrastado no Inspector, tenta achar automaticamente
        if (ds == null)
        {
#if UNITY_2023_1_OR_NEWER
            ds = UnityEngine.Object.FindFirstObjectByType<DialogueSystem>();
#else
            ds = FindObjectOfType<DialogueSystem>();
#endif
        }

        if (ds != null)
        {
            ds.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning("DialogueSystem não encontrado na cena.");
        }
    }

    // Exemplo: iniciar diálogo ao entrar no trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ja_passou)
        {
            TriggerDialogue();
            if (!passar_mais_de_uma_vez)
                ja_passou = true;
        }
    }
}
