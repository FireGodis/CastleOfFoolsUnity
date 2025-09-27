using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
    public string characterName;

    [TextArea(2, 5)]
    public string text;

    public Sprite portrait;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string dialogueName;
    public List<DialogueLine> lines;
}

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text dialogueText;     // UI do texto
    public TMP_Text characterNameText;// UI do nome do personagem
    public Image characterPortrait;   // Se quiser mostrar a imagem do personagem
    public Button nextButton;
    public CharacterController3D player;      // Referência ao jogador para desativar o movimento
    public animacaoUIdireita animaUI; // Referência ao script de animação UI

    public float typeSpeed = 0.03f;

    // agora usamos DialogueData (ScriptableObject)
    private DialogueData currentDialogue;
    private int lineIndex = 0;
    private bool isTyping = false;
    private string currentLine;

    private Coroutine typingCoroutine;


    void Start()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(HandleNext); // sem parâmetros

        HideDialogue();
    }

    // NOTA: agora recebe DialogueData
    public void StartDialogue(DialogueData dialogueData)
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("StartDialogue recebeu null DialogueData");
            return;
        }

        currentDialogue = dialogueData;
        lineIndex = 0;
        ShowDialogue();
        DisplayNextLine(); // agora DisplayNextLine não precisa de parâmetro
    }




    private IEnumerator Tempo_de_espera()
    {
        yield return new WaitForSeconds(1);  // espera 1 segundos
    }

    void ShowDialogue()
    {
        
        gameObject.SetActive(true);
        Tempo_de_espera();
        player.pode_mover = false;
    }

    void HideDialogue()
    {
        
        gameObject.SetActive(false);
    }

    void HandleNext()
    {
        if (isTyping)
        {
            // Termina imediatamente
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            dialogueText.text = currentLine;
            isTyping = false;
        }
        else
        {
            DisplayNextLine();
        }
    }

    void DisplayNextLine()
    {
        if (currentDialogue == null || currentDialogue.lines == null)
        {
            EndDialogue();
            return;
        }

        if (lineIndex < currentDialogue.lines.Count)
        {
            DialogueLine line = currentDialogue.lines[lineIndex];
            characterNameText.text = line.characterName;
            currentLine = line.text;

            // 🔹 Atualiza retrato
            if (line.portrait != null)
            {
                characterPortrait.sprite = line.portrait;
                characterPortrait.gameObject.SetActive(true);
            }
            else
            {
                characterPortrait.gameObject.SetActive(false); // oculta se não tiver sprite
            }

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(currentLine));
            lineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        HideDialogue();

        
        if (currentDialogue != null && currentDialogue.dialogueName == "AprenderWASD")
        {
            Debug.Log("Diálogo inicial 1 concluído!");
            animaUI.MoverIdaVolta();
        }
        
        player.pode_mover = true;
        
    }
}
