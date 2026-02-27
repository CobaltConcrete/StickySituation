using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Attach to Human Player Game Object")]
    public GameObject human;

    [SerializeField]
    [Tooltip("Attach to Slime Player Game Object")]
    public GameObject slime;

    [SerializeField]
    [Tooltip("Rope Segment Game Object")]
    public GameObject ropeSegmentPrefab;

    [SerializeField]
    [Tooltip("Width of each Rope Segment")]
    public float ropeWidth = 0.10f;

    [SerializeField]
    [Tooltip("How fast the rope increses/decreses in length")]
    public float ropeAdjustSpeed = 5f;
    [SerializeField]
    [Tooltip("Min Rope length")]
    public float minLength = 0.5f;
    [SerializeField]
    [Tooltip("Max Rope length")]
    public float maxLength = 20f;

    Rigidbody2D segment;

    LineRenderer line;
    DistanceJoint2D tether;

    Rigidbody2D humanRB;
    Rigidbody2D slimeRB;

    float ropeLength;

    void Start()
    {
        humanRB = human.GetComponent<Rigidbody2D>();
        slimeRB = slime.GetComponent<Rigidbody2D>();

        // Line Renderer
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = ropeWidth;
        line.endWidth = ropeWidth;

        // Distance Joint 2D
        ropeLength = Vector2.Distance(human.transform.position, slime.transform.position);
        tether = human.AddComponent<DistanceJoint2D>();
        tether.connectedBody = slimeRB;
        tether.autoConfigureDistance = false;
        tether.maxDistanceOnly = true;
        tether.distance = ropeLength;

        GameObject seg = Instantiate(ropeSegmentPrefab);
        seg.layer = LayerMask.NameToLayer("Rope");
        segment = seg.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
        DrawRope();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Period))
            ropeLength += ropeAdjustSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Comma))
            ropeLength -= ropeAdjustSpeed * Time.deltaTime;

        ropeLength = Mathf.Clamp(ropeLength, minLength, maxLength);
        tether.distance = ropeLength;
    }

    void DrawRope()
    {
        Vector2 a = human.transform.position;
        Vector2 b = slime.transform.position;
        Vector2 mid = (a + b) * 0.5f;
        
        segment.MovePosition(mid);
        line.positionCount = 3;
        line.SetPosition(0, a);
        line.SetPosition(1, mid);
        line.SetPosition(2, b);
    }
}