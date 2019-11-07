using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[ExecuteInEditMode]
public class Frame : MonoBehaviour
{
    [Header("Initializations")]
    public Camera targetCamera = null;
    public RectTransform videoCanvas = null;
    public VideoPlayer videoPlayer = null;
    public Animator animator  = null;
    [Range(0f, 1f)]
    public float interpolationRate = 0.05f;
    public VideoClip[] videoClips = null;

    [Header("Slider Data")]
    public Transform sliderTransform = null;
    public int dataFPS = 100;

    [Header("Slider Settings")]
    public float lineLength = 25f;

    [Header("Graphical Settings")]
    public GUISkin guiSkin = null;

    private bool DISPLAY_SLIDER_POSITION = true;
    private Vector2 SLIDER_POSITION = Vector2.zero;

    private float CAMERA_HEIGHT;
    private float CAMERA_WIDTH;
    private bool USE_INTERPOLATION = false;

    private const string ANIM_01_TRIGGER = "ANIM_01";

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);

        CAMERA_HEIGHT = 2f * targetCamera.orthographicSize;
        CAMERA_WIDTH = CAMERA_HEIGHT * targetCamera.aspect;

        InvokeRepeating("ReadSliderPositionData", 0f, 1.0f / dataFPS);
    }

    private void LateUpdate()
    {
        if (sliderTransform != null)
        {
            if (USE_INTERPOLATION)
            {
                targetCamera.transform.position = Vector3.Lerp(
                    targetCamera.transform.position, 
                    new Vector3(SLIDER_POSITION.x + (CAMERA_WIDTH / 2), targetCamera.transform.position.y, targetCamera.transform.position.z),
                    interpolationRate);
            } else
            {
                targetCamera.transform.position = new Vector3(SLIDER_POSITION.x + (CAMERA_WIDTH / 2), targetCamera.transform.position.y, targetCamera.transform.position.z);
            }

        }
    }

    private void ReadSliderPositionData()
    {
        if (sliderTransform != null)
        {
            SLIDER_POSITION = sliderTransform.position;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(32, 32, 250, 32), "CAMERA_HEIGHT:\t" + CAMERA_HEIGHT, guiSkin.GetStyle("label"));
        GUI.Label(new Rect(32, 64, 250, 32), "CAMERA_WIDTH:\t\t" + CAMERA_WIDTH, guiSkin.GetStyle("label"));

        GUI.Label(new Rect(32, 96, 250, 32), "SLIDER_POSITION:\t" + SLIDER_POSITION, guiSkin.GetStyle("label"));

        DISPLAY_SLIDER_POSITION = GUI.Toggle(new Rect(32, 160, 250, 32), DISPLAY_SLIDER_POSITION, "Display Slider Position", guiSkin.GetStyle("toggle"));

        USE_INTERPOLATION = GUI.Toggle(new Rect(32, 202, 250, 32), USE_INTERPOLATION, "Use Interpolation", guiSkin.GetStyle("toggle"));

        GUI.Label(new Rect(25, 266, 250, 32), "Interpolation Rate:\t\t" + interpolationRate, guiSkin.GetStyle("label"));

        interpolationRate = GUI.HorizontalSlider(new Rect(550, 285, 250, 32), interpolationRate, 0f, 1f);

        if (GUI.Button(new Rect(25, 324, 250, 64), "Play Anim 01", guiSkin.button))
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            animator.SetTrigger(ANIM_01_TRIGGER);
        }

        if (GUI.Button(new Rect(25, 484, 250, 64), "Restart Scene", guiSkin.button))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (DISPLAY_SLIDER_POSITION)
        {
            Gizmos.DrawWireSphere(SLIDER_POSITION, 0.1f);

            Gizmos.DrawLine(new Vector3(0f, SLIDER_POSITION.y), new Vector3(lineLength, SLIDER_POSITION.y));
            Gizmos.DrawLine(new Vector3(0f, SLIDER_POSITION.y), new Vector3(-1 * lineLength, SLIDER_POSITION.y));
        }
    }

}
