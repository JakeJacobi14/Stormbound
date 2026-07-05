using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; // For SortingGroup

public class ExternalCharge : MonoBehaviour
{
    private GameObject TestCharge;
    public float power;
    [HideInInspector] public bool isDragging = false;
    private GameController GameController;
    private UndoManager UndoManager;
    [SerializeField] private GameObject ArrowGameObject;
    [SerializeField] private float ArrowSizeScale = 0.1f;
    [SerializeField] private float ArrowLengthScale = 1.2f;
    [SerializeField] private float ArrowMinSize = 0.025f;
    [SerializeField] private Color RedColor;
    [SerializeField] private Color BlueColor;
    [SerializeField] private Color GrayColor;
    [SerializeField] private LayerMask MufflerLayer; 
    [SerializeField] private bool ShowGrayedArrows;
    public bool isPositive;
    public bool SpawnedIn = false;
    [SerializeField] int defaultLayer = 0;
    [SerializeField] float ArrowDistance = 0.8f;

    private GameObject arrow;
    private SortingGroup sortingGroup;
    private Collider2D myCollider;
    private Vector2 offset;

    void Awake()
    {
        GameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        UndoManager = GameObject.FindWithTag("UndoManager").GetComponent<UndoManager>();
        TestCharge = GameObject.FindWithTag("Test Charge");
        GameController.NumCharges++;

        sortingGroup = GetComponent<SortingGroup>();
        if (sortingGroup == null)
        {
            sortingGroup = gameObject.AddComponent<SortingGroup>();
        }
        sortingGroup.sortingOrder = defaultLayer;

        myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (SpawnedIn)
            AudioSource.PlayClipAtPoint(GameController.RandomPopSound(), Vector2.zero);
    }

    void FixedUpdate()
    {
        HandlePhysics();
    }

    void Update()
    {
        HandleArrows();
        HandleDragging();
    }

    bool IsBlockedByMuffler()
    {
        Vector2 direction = (TestCharge.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(TestCharge.transform.position, transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, MufflerLayer);
        return hit.collider != null;
    }

    void HandlePhysics()
    {
        if (IsBlockedByMuffler()) return;

        Vector2 direction = (TestCharge.transform.position - transform.position).normalized;
        float scaling = 1f / Mathf.Max(Vector2.Distance(TestCharge.transform.position, transform.position), 1f);
        TestCharge.GetComponent<Rigidbody2D>().AddForce(direction * scaling * power * Time.deltaTime, ForceMode2D.Impulse);
    }

    void HandleArrows()
    {
        Destroy(arrow);
        // Debug.Log(GameController.DrawArrows);
        if (!GameController.DrawArrows) return;

        bool blocked = IsBlockedByMuffler();
        if (blocked && !ShowGrayedArrows) return;

        Vector2 direction = (TestCharge.transform.position - transform.position).normalized;
        float scaling = 1f / Mathf.Max(Vector2.Distance(TestCharge.transform.position, transform.position), 1f);

        arrow = Instantiate(ArrowGameObject, TestCharge.transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + (power > 0 ? 0 : 180);
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

        float newScale = (scaling + ArrowMinSize) * ArrowSizeScale;
        arrow.transform.localScale = (newScale <= 1) ? new Vector2(newScale, newScale) : Vector2.one;

        arrow.transform.GetChild(1).localScale = new Vector2(newScale * ArrowLengthScale, 1);
        arrow.transform.position = TestCharge.transform.position +
            (Vector3)direction * ((scaling *ArrowSizeScale * ArrowLengthScale)+ArrowDistance) * (power > 0 ? 1 : -1);

        // arrow color
        Color arrowColor = blocked ? GrayColor : (power > 0 ? RedColor : BlueColor);
        arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().color = arrowColor;
        arrow.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = arrowColor;
    }

    void HandleDragging()
    {
        if (Input.GetMouseButtonDown(0) && !GameController.IsGameRunning())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                DemoteOverlappingCharges();
                UpdateChargeLayer(defaultLayer + 1);
                offset = (Vector2)transform.position - mousePos;
            }
        }

        if (isDragging && Input.GetMouseButton(0) && !GameController.IsGameRunning())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos + offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            CheckForDeletion();
            UndoManager.SaveScreen();
        }
    }

    void CheckForDeletion()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hitObjects = Physics2D.OverlapPointAll(mouseWorldPos);
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Box") && hit.gameObject != gameObject)
            {
                if (hit.bounds.Contains(new Vector3(transform.position.x, transform.position.y, hit.transform.position.z)))
                {
                    AudioSource.PlayClipAtPoint(GameController.RandomPopSound(), Vector2.zero);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

    void UpdateChargeLayer(int layer)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = layer;
        if (sortingGroup != null)
            sortingGroup.sortingOrder = layer;
    }

    void DemoteOverlappingCharges()
    {
        if(myCollider == null)
            return;

        GameObject[] charges = GameObject.FindGameObjectsWithTag("Charge");
        foreach (GameObject charge in charges)
        {
            if (charge == gameObject) continue;

            Collider2D otherCol = charge.GetComponent<Collider2D>();
            if (otherCol != null && myCollider.bounds.Intersects(otherCol.bounds))
            {
                // Demote overlapping charge
                SpriteRenderer otherSR = charge.GetComponent<SpriteRenderer>();
                if (otherSR != null)
                    otherSR.sortingOrder = defaultLayer;
                SortingGroup otherSG = charge.GetComponent<SortingGroup>();
                if (otherSG != null)
                    otherSG.sortingOrder = defaultLayer;
            }
        }
    }

    void OnDestroy()
    {
        GameController.NumCharges--;
        Destroy(arrow);
    }
}
