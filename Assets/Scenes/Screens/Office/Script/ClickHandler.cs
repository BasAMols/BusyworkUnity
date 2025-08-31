using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAreaHandler : MonoBehaviour, IPointerDownHandler
{
    public PlayerController2D player; // assign in Inspector
    public Camera cam;                 // assign Main Camera
    [SerializeField] private GameObject indicatorPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Convert screen position to world
        Vector3 screen = eventData.position;
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screen.x, screen.y, -cam.transform.position.z));
        player.MoveTo(world);

        Instantiate(indicatorPrefab, world, Quaternion.identity, gameObject.transform);

    }
}