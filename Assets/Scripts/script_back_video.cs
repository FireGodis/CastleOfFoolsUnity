using UnityEngine;
using UnityEngine.Video;



public class script_back_video : MonoBehaviour
{

    public VideoPlayer video_menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        video_menu.source = VideoSource.Url;
        video_menu.url = Application.streamingAssetsPath + "/background loop BN.mp4";
        video_menu.Play();
        video_menu.isLooping = false;
        video_menu.isLooping = true;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
