using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CinematicAspect : MonoBehaviour
{
    [Header("Target framing")]
    [Min(0.01f)] public float targetWidth = 268f;
    [Min(0.01f)] public float targetHeight = 100f;

    [Header("Options")]
    public bool applyInEditMode = true;
    public bool updateContinuously = true;

    private Camera _cam;
    private int _lastScreenWidth = -1;
    private int _lastScreenHeight = -1;

    private float TargetAspect => targetWidth / targetHeight;

    private void OnEnable()
    {
        _cam = GetComponent<Camera>();
        ApplyAspect();
    }

    private void OnValidate()
    {
        targetWidth = Mathf.Max(0.01f, targetWidth);
        targetHeight = Mathf.Max(0.01f, targetHeight);

        if (!Application.isPlaying && !applyInEditMode)
            return;

        _cam = GetComponent<Camera>();
        ApplyAspect();
    }

    private void Update()
    {
        if (!Application.isPlaying && !applyInEditMode)
            return;

        if (!updateContinuously)
            return;

        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
        {
            ApplyAspect();
        }
    }

    private void ApplyAspect()
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();

        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = TargetAspect;

        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;

        // scaleHeight < 1 => écran plus "étroit" que le framing cible
        // => bandes noires en haut / bas
        float scaleHeight = screenAspect / targetAspect;

        if (scaleHeight < 1f)
        {
            Rect rect = new Rect
            {
                x = 0f,
                width = 1f,
                height = scaleHeight,
                y = (1f - scaleHeight) * 0.5f
            };

            _cam.rect = rect;
        }
        else
        {
            // Cas inverse : écran plus large que le framing cible
            // => bandes noires à gauche / droite
            float scaleWidth = 1f / scaleHeight;

            Rect rect = new Rect
            {
                x = (1f - scaleWidth) * 0.5f,
                width = scaleWidth,
                y = 0f,
                height = 1f
            };

            _cam.rect = rect;
        }
    }

    private void OnDisable()
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();

        if (_cam != null)
            _cam.rect = new Rect(0f, 0f, 1f, 1f);
    }
}