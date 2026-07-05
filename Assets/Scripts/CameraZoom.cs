using UnityEngine;
using DG.Tweening;

public class CameraZoom : MonoBehaviour
{
    public Camera cam;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float returnTime = 0.5f;
    public float dragSpeed = 0.1f;
    public GameController GameController;
    
    private Vector3 originalPosition;
    private float originalSize;
    private Vector3 dragOrigin;
    private bool isDragging;
    private bool returningToOrigin;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
        
        originalPosition = cam.transform.position;
        originalSize = cam.orthographicSize;
    }

    void Update()
    {
        if (GameController.IsInUI) return;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            float newSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            float zoomFactor = newSize / cam.orthographicSize;
            
            cam.transform.position += (mouseWorldPos - cam.transform.position) * (1 - zoomFactor);
            cam.orthographicSize = newSize;
            returningToOrigin = false;

        }
        else if (cam.orthographicSize >= originalSize && cam.transform.position != originalPosition && !returningToOrigin)
        {
            returningToOrigin = true;

            cam.transform.DOMove(originalPosition, returnTime).SetEase(Ease.OutQuad).SetUpdate(true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            returningToOrigin = false;

        }
        if (Input.GetMouseButton(1) && isDragging)
        {
            returningToOrigin = true;

            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }
    }
    public void ResetCamera()
    {
        cam.orthographicSize = originalSize;
        cam.transform.position = originalPosition;
    }
}
