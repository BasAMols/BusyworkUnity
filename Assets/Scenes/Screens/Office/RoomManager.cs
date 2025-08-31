using UnityEngine;

public class RoomManager : BaseScreen
{
    private float[] screenDim = { -1, 1, 1, -1 };
    private float[] prevScreenDim = { -1, 1, 1, -1 };
    private float[] easedScreenDim = { -1, 1, 1, -1 };
    private float[] maxScreenDim;
    public override float[] GetScreenDimension() => easedScreenDim;
    [SerializeField] protected PlayerController2D player;
    [SerializeField] protected ClickAreaHandler clickHandler;

    private RoomBase[] rooms;
    protected void OnValidate()
    {
        rooms = Object.FindObjectsByType<RoomBase>(FindObjectsSortMode.None);
        float left = Mathf.Infinity;
        float right = -Mathf.Infinity;
        float top = -Mathf.Infinity;
        float bottom = Mathf.Infinity;
        foreach (var room in rooms)
        {
            float[] roomDim = room.GetRoomDimension();
            if (roomDim[1] - roomDim[0] == 0 || roomDim[2] - roomDim[3] == 0) continue;
            left = Mathf.Min(left, roomDim[0]);
            right = Mathf.Max(right, roomDim[1]);
            top = Mathf.Max(top, roomDim[2]);
            bottom = Mathf.Min(bottom, roomDim[3]);
        }
        float width = right - left;
        float height = top - bottom;

        clickHandler.transform.position = new Vector3(left + width / 2, bottom + height / 2, 0);
        clickHandler.transform.localScale = new Vector3(width, height, 1);

        maxScreenDim = new float[] { left, right, top, bottom };
    }

    private void Update()
    {
        if (!Application.isPlaying) return;
        float left = Mathf.Infinity;
        float right = -Mathf.Infinity;
        float top = -Mathf.Infinity;
        float bottom = Mathf.Infinity;
        prevScreenDim = screenDim;
        foreach (var room in rooms)
        {
            float[] roomDim = room.GetRoomDimension();
            room.SetInRoom(player.transform.position);
            if (!room.InRoom) continue;
            if (roomDim[1] - roomDim[0] == 0 || roomDim[2] - roomDim[3] == 0) continue;
            left = Mathf.Min(left, roomDim[0]);
            right = Mathf.Max(right, roomDim[1]);
            top = Mathf.Max(top, roomDim[2]);
            bottom = Mathf.Min(bottom, roomDim[3]);
        }
        float width = right - left;
        float height = top - bottom;

        if (left != prevScreenDim[0] || right != prevScreenDim[1] || top != prevScreenDim[2] || bottom != prevScreenDim[3])
        {
            screenDim = new float[] { left, right, top, bottom };
        }

    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            easedScreenDim[i] = Mathf.SmoothStep(easedScreenDim[i], screenDim[i], Time.fixedDeltaTime * 5f);
        }
    }
}
