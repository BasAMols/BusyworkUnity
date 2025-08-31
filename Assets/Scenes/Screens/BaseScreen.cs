using UnityEngine;
using System.Collections;

[ExecuteAlways]
public abstract class BaseScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteMask spriteMask;
    public abstract float[] GetScreenDimension();
}