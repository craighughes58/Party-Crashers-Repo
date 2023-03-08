using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] MusicPoints mp;
    [SerializeField] AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = gameObject.GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchSongs(mp.songNumber);
    }

    private void SwitchSongs(int song)
    {
        switch (song)
        {
            case 1:
                break;
            case 2:
                // Play song 2
                break;
            case 3:
                // Play song 3
                break;
            case 4:
                // Play song 4
                break;
            case 5:
                // Play song 5
                break;
            case 6:
                // Play song 6
                break;
            case 7:
                // Play song 7
                break;
            default:
                break;
        }
        //print("Crossing Music Point " + song.ToString());
    }
}
