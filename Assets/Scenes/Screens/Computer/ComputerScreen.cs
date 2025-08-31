using UnityEngine;

public class ComputerScreen : BaseScreen
{
    [SerializeField] private BoolVariable atDesk;

    public override float[] GetScreenDimension() => new float[] { -1, 1, 1, -1 };

    private void Awake()
    {
        atDesk.AddListener(OnAtDeskChanged);
        OnAtDeskChanged(atDesk.Value);
    }

    private void OnAtDeskChanged(bool value)
    {
        // Collapsed = !value;
    }
}
