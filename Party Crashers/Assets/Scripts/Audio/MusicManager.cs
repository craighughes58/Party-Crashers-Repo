using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] MusicPoints mp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayMusic(mp.songNumber);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MusicTrigger"))
        {

        }
    }

    private void PlayMusic(int song)
    {
        switch (song)
        {
            case 1:
                // Play song 1
                print(song);
                break;
            case 2:
                // Play song 2
                print(song);
                break;
            case 3:
                // Play song 3
                print(song);
                break;
            case 4:
                // Play song 4
                print(song);
                break;
            case 5:
                // Play song 5
                print(song);
                break;
            case 6:
                // Play song 6
                print(song);
                break;
            case 7:
                // Play song 7
                print(song);
                break;
            default:
                break;
        }
    }
}
