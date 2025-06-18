using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
           DontDestroyOnLoad(gameObject);

            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null && !audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}



