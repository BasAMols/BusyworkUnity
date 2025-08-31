using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    private BaseScreen[] screens;
    private float buffer = 0.5f;
    private int lastWidth;
    private int lastHeight;
    private Camera cam;
    private void OnValidate()
    {
        screens = Object.FindObjectsByType<BaseScreen>(FindObjectsSortMode.None);
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        cam = Camera.main;

    }

    private void Update()
    {
        if (!Application.isPlaying) return;
        float left = Mathf.Infinity;
        float right = -Mathf.Infinity;
        float top = -Mathf.Infinity;
        float bottom = Mathf.Infinity;
        foreach (var screen in screens)
        {
            float[] screenDim = screen.GetScreenDimension();
            if (screenDim[1] - screenDim[0] == 0 || screenDim[2] - screenDim[3] == 0) continue;
            left = Mathf.Min(left, screenDim[0]);
            right = Mathf.Max(right, screenDim[1]);
            top = Mathf.Max(top, screenDim[2]);
            bottom = Mathf.Min(bottom, screenDim[3]);
        }
        float width = right - left + buffer * 2;
        float height = top - bottom + buffer * 2;


        // 2) Center of the bounds
        float cx = (left + right) * 0.5f;
        float cy = (top + bottom) * 0.5f;

        // 3) Compute required orthographic size to fit both width & height
        //    - vertical fit: ortho = height / 2
        //    - horizontal fit: ortho = (width / 2) / aspect
        float requiredOrtho = Mathf.Max(
            height * 0.5f,
            (width * 0.5f) / cam.aspect
        );

        // 4) Apply to camera
        cam.orthographicSize = requiredOrtho;
        cam.transform.position = new Vector3(cx, cy, cam.transform.position.z);
    }


}
