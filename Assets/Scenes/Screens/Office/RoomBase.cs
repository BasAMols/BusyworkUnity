using UnityEngine;
using System.Collections;

[ExecuteAlways]
public abstract class RoomBase : MonoBehaviour
{
    [SerializeField] protected GameObject colliderWrapper = null;
    protected Collider2D[] colliders = { };
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteMask mask;
    [SerializeField] protected SpriteRenderer overlay;
    [SerializeField] protected Vector2 roomAnchor = new Vector2(0.5f, 0.5f);

    private float[] roomDim = { 0, 0, 0, 0 };
    public float[] GetRoomDimension() => roomDim;
    private float inRoomFloat = 0;
    private bool inRoom = false;
    public bool InRoom
    {
        get => inRoom;
        set => inRoom = value;
    }

    private void OnValidate()
    {
        roomDim = new float[] {
            spriteRenderer.transform.position.x - spriteRenderer.transform.localScale.x / 2,
            spriteRenderer.transform.position.x + spriteRenderer.transform.localScale.x / 2,
            spriteRenderer.transform.position.y + spriteRenderer.transform.localScale.y / 2,
            spriteRenderer.transform.position.y - spriteRenderer.transform.localScale.y / 2
        };
        mask.transform.localScale = new Vector3(1f, 1f, 1f);
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0f);
        if (colliderWrapper != null)
        {
            colliders = colliderWrapper.GetComponentsInChildren<Collider2D>();
        }
        inRoom = true;

    }

    public void SetInRoom(Vector3 position)
    {

        if (colliders.Length == 0)
        {
            inRoom = true;
            return;
        }

        bool temp = false;
        foreach (var collider in colliders)
        {
            if (collider.bounds.Contains(position))
            {
                temp = true;
                break;
            }
        }

        inRoom = temp;
    }

    private void FixedUpdate()
    {
        inRoomFloat = Mathf.MoveTowards(inRoomFloat, inRoom ? 1f : 0f, Time.fixedDeltaTime * 2f);
        // mask.transform.localScale = new Vector3(Mathf.SmoothStep(0, 1, inRoomActual), Mathf.SmoothStep(0, 1, inRoomActual), 1f);
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, Mathf.SmoothStep(0, 1, 0.85f-inRoomFloat*0.85f));
    }

    // private Coroutine collapseRoutine;

    // private Vector2 screenDimensions;
    // public Vector2 ScreenDimensions
    // {
    //     get => screenDimensions;
    //     set
    //     {
    //         screenDimensions = value;
    //     }
    // }
    // private Vector2 screenPosition;
    // public Vector2 ScreenPosition
    // {
    //     get => screenPosition;
    //     set
    //     {
    //         screenPosition = value;
    //     }
    // }
    // public abstract Vector2 InitialDimensions { get; }
    // public abstract Vector2 InitialPosition { get; }
    // public abstract Vector2 AnchorPoint { get; }

    // public abstract Color ScreenColor { get; }
    // public abstract CollapseAxis CollapseMode { get; }
    // public abstract float InitialCollapse { get; }

    // // Which axis to collapse along
    // public enum CollapseAxis { X, Y, Both }


    // // Collapse toggle
    // public bool Collapsed
    // {
    //     get => collapsed;
    //     set
    //     {
    //         if (collapsed != value)
    //         {
    //             collapsed = value;

    //             if (Application.isPlaying)
    //             {
    //                 if (collapseRoutine != null)
    //                     StopCoroutine(collapseRoutine);

    //                 collapseRoutine = StartCoroutine(AnimateCollapse(collapsed));
    //             }
    //             else
    //             {
    //                 ApplyCollapseInstant(collapsed ? 0f : 1f);
    //             }
    //         }
    //     }
    // }
    // private bool collapsed;


    // private void Awake()
    // {
    //     spriteRenderer.transform.localScale = new Vector3(InitialDimensions.x, InitialDimensions.y, 1f);
    //     ApplyCollapseInstant(InitialCollapse);
    // }


    // protected virtual void OnValidate()
    // {
    //     if (spriteRenderer == null) return;

    //     spriteMask.sprite = spriteRenderer.sprite;

    //     // sync size + position
    //     var fullScale = new Vector3(InitialDimensions.x, InitialDimensions.y, 1f);
    //     spriteRenderer.transform.localScale = fullScale;
    //     spriteRenderer.transform.localPosition = new Vector3(
    //         InitialPosition.x,
    //         InitialPosition.y,
    //         0f);
    //     spriteRenderer.color = ScreenColor;
    //     spriteMask.transform.localScale = fullScale;
    //     spriteMask.transform.localPosition = spriteRenderer.transform.localPosition;

    // }

    // private IEnumerator AnimateCollapse(bool toCollapsed)
    // {
    //     float duration = 1f;
    //     float elapsed = 0f;

    //     float start = toCollapsed ? 1f : 0f;
    //     float end = toCollapsed ? 0f : 1f;

    //     while (elapsed < duration)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t = Mathf.SmoothStep(start, end, elapsed / duration);
    //         ApplyCollapseInstant(t);
    //         yield return null;
    //     }

    //     ApplyCollapseInstant(end);
    // }

    // protected void ApplyCollapseInstant(float factor)
    // {
    //     Vector3 full = new Vector3(InitialDimensions.x, InitialDimensions.y, 1f);
    //     Vector3 scale = full;

    //     switch (CollapseMode)
    //     {
    //         case CollapseAxis.X:
    //             scale.x = InitialDimensions.x * factor;
    //             break;
    //         case CollapseAxis.Y:
    //             scale.y = InitialDimensions.y * factor;
    //             break;
    //         case CollapseAxis.Both:
    //             scale = full * factor;
    //             break;
    //     }

    //     spriteMask.transform.localScale = scale;
    //     screenDimensions = scale;
    // }

}