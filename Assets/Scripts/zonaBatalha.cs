using UnityEngine;

public class zonaBatalha : MonoBehaviour
{
    public script_botoes_menu botoes_script;

    public AudioSource audio_sonho;
    public AudioSource audio_sonho_intenso;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            botoes_script.fadein_audio(audio_sonho);
            botoes_script.fadeout_audio(audio_sonho_intenso);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            botoes_script.fadeout_audio(audio_sonho);
            botoes_script.fadein_audio(audio_sonho_intenso);
        }
    }
}
