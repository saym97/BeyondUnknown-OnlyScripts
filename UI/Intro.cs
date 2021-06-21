using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float firstVideoLength;
    public VideoClip secondVideo;
    private WaitForSeconds wait;
    bool secondvideoisPlaying = false;
    bool firstVideoplaying = true;
    // Start is called before the first frame update
    void Start()
    {
        wait = new WaitForSeconds(firstVideoLength);
        StartCoroutine(WaitforVideo());
    }

    IEnumerator WaitforVideo() {
        yield return wait;
        videoPlayer.clip = secondVideo;
        videoPlayer.Play();
        firstVideoplaying = false;
        secondvideoisPlaying = true;
        // SceneManager.LoadScene("Base Scene");
    }
    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.anyKeyDown) {
            if (firstVideoplaying)SkipFirstVideo();
            else if (secondvideoisPlaying) {
                videoPlayer.Stop();
                CancelInvoke();
                SceneManager.LoadScene("Base Scene");
                Debug.Log("second video skipped");

            }

        }
    }
    void SkipFirstVideo() {
        if (!firstVideoplaying) return;
        StopCoroutine(WaitforVideo());
        videoPlayer.Stop();
        videoPlayer.clip = secondVideo;
        videoPlayer.Play();
        Invoke("Load", 31f);
        firstVideoplaying = false;
        secondvideoisPlaying = true;
        Debug.Log("First video skipped");
    }
    void Load() {
        SceneManager.LoadScene("Base Scene");
    }
}
