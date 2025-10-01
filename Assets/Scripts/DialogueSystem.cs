using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class DialogueLine
{
    public string characterName;

    [TextArea(2, 5)]
    public string text;

    public Sprite portrait;
    public AudioClip voiceClip; // Se quiser adicionar áudio
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
    public AudioSource audioSource; // Fonte de áudio para tocar clipes de voz
    public AudioClip voz_padrao; // Clip de voz padrão, se necessário
    public botao_opcoes script_op; // Referência ao script de opções para verificar se a dublagem está ativada
    public GameObject obj_alune1;

    
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
        if (currentDialogue != null && currentDialogue.dialogueName == "AprenderWASD")
        {
            Debug.Log("Iniciando diálogo inicial 1");
            

        }
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

            if (line.voiceClip != null && script_op.dublagem)
            {
                
                audioSource.clip = line.voiceClip;
                audioSource.Play();
                
            }
            else if (!script_op.dublagem)
            {
                audioSource.clip = voz_padrao;
                audioSource.Play();
                audioSource.volume = 0.5f;
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
        if (!script_op.dublagem)
        {
            audioSource.Stop();
        }
    }

   

    void EndDialogue()
    {
        

        
        if (currentDialogue != null && currentDialogue.dialogueName == "AprenderWASD")
        {
            Debug.Log("Diálogo inicial 1 concluído!");
            animaUI.MoverIdaVolta();
        }
        if (currentDialogue != null && currentDialogue.dialogueName == "Teste")
        {
            obj_alune1.GetComponent<SpriteRenderer>().DOColor(Color.red, 1.5f);
            obj_alune1.transform.DOScaleX(0f, 1.5f); // só eixo X
            obj_alune1.transform.DOMoveY(1f, 1.5f);
        }
        HideDialogue();


        player.pode_mover = true;
        
    }
    

    
}
