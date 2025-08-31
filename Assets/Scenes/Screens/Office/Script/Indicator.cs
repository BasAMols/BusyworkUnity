using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Indicator : MonoBehaviour
{
    private float instantiationTime;
    private float maxTime = 0.5f;
    private float maxSize = 0.2f;
    private SpriteRenderer spriteRenderer;
    private Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instantiationTime = Time.time;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceInstantiation = Time.time - instantiationTime;
        float eased = Mathf.SmoothStep(0, 1, timeSinceInstantiation/maxTime);

        transform.localScale = new Vector3(eased*maxSize*0.65f, eased*maxSize, 0);
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f - eased);

        if (timeSinceInstantiation >= maxTime){   
            Destroy(gameObject);
        }
    }
}
