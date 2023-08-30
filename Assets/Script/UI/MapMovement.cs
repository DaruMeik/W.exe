using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public Camera cam;
    public GameObject Left;
    public GameObject Right;
    [SerializeField] private EventBroadcast eventBroadcast;
    private void OnEnable()
    {
        eventBroadcast.EnterUINoti();
    }
    private void OnDisable()
    {
        eventBroadcast.ExitUINoti();
    }
    private void Update()
    {
        Vector2 dir = PlayerControl.Instance.pInput.Player.Move.ReadValue<Vector2>();
        cam.transform.position = new Vector3(Mathf.Max(4.5f, Mathf.Min(11.5f, cam.transform.position.x + dir.x * 3f * Time.deltaTime)), cam.transform.position.y, cam.transform.position.z);
        if (cam.transform.position.x < 11.25f)
            Right.SetActive(true);
        else
            Right.SetActive(false);
        if (cam.transform.position.x > 4.75f)
            Left.SetActive(true);
        else
            Left.SetActive(false);
    }
}
