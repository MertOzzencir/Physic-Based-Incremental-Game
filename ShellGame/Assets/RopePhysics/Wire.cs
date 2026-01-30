using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Wire : MonoBehaviour
{
    [SerializeField] private Transform startPoint, endPoint, segmentsParent;
    public int segmentCount = 10;
    public float totalLength = 10;
    [SerializeField] private float radius = 0.5f;

    [SerializeField] private bool usePhysics;
    [SerializeField] private float totalWeight = 10f;
    [SerializeField] private float drag = 1f;
    [SerializeField] private float angularDrag = 1f;

    [Header("Mesh Settings")]
    [SerializeField] private int sides = 4;

    public bool CanUpdate;
    private Transform[] segments;
    private Vector3[] vertices;
    private int[,] vertexIndicesMap;

    private WireMeshData meshdata;
    private Mesh mesh;
    private bool createTriangles;

    private MeshRenderer mRenderer;
    private MeshFilter mFilter;

    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        mFilter = GetComponent<MeshFilter>();
        segments = new Transform[segmentCount];
        vertices = new Vector3[segmentCount * sides * 3];
        GenerateSegments();
    }

    private void Update()
    {
        if (CanUpdate)
        {
            DestroySegments();
            segments = new Transform[segmentCount];
            GenerateSegments();
        }


        UpdateMesh();
    }

    private void GenerateSegments()
    {
        JoinSegments(startPoint, null, true);
        Transform previousTransform = startPoint;
        Vector3 direction = endPoint.position - startPoint.position;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject nextSegment = new GameObject($"segment{i}");
            nextSegment.transform.parent = segmentsParent;
            segments[i] = nextSegment.transform;

            Vector3 position = previousTransform.position + direction / segmentCount;
            nextSegment.transform.position = position;

            JoinSegments(nextSegment.transform, previousTransform);
            previousTransform = nextSegment.transform;
        }
        JoinSegments(endPoint, previousTransform, true, true);
        SetupCollisionAvoidance();
        GenerateMesh();

    }
    public void UpdateLength()
    {
        int i = 0;
        foreach (var a in segments)
        {
            if (i == 0)
            {
                i++;
                continue;
            }
            i++;
            a.GetComponent<ConfigurableJoint>().connectedAnchor = Vector3.forward * (totalLength / segmentCount);
        }
        endPoint.transform.GetComponent<ConfigurableJoint>().connectedAnchor = Vector3.forward * 0.1f;
    }

    private void JoinSegments(Transform current, Transform connectedTransform, bool isKinematic = false, bool isCloseConnected = false)
    {
        if (current.GetComponent<Rigidbody>() == null)
        {

            Rigidbody segmentRigidbody = current.AddComponent<Rigidbody>();
            segmentRigidbody.isKinematic = isKinematic;
            segmentRigidbody.mass = totalWeight / segmentCount;
            segmentRigidbody.linearDamping = drag;
            segmentRigidbody.angularDamping = angularDrag;
            segmentRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            if (!isKinematic)
                segmentRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            else
                segmentRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        if (usePhysics)
        {
            SphereCollider collider = current.GetComponent<SphereCollider>();
            if (collider == null)
            {
                collider = current.AddComponent<SphereCollider>();
            }

            collider.radius = radius;
            collider.transform.gameObject.layer = 8;
        }
        if (connectedTransform != null)
        {
            ConfigurableJoint segmentJoint = current.GetComponent<ConfigurableJoint>();
            if (segmentJoint == null)
            {
                segmentJoint = current.AddComponent<ConfigurableJoint>();
            }

            segmentJoint.connectedBody = connectedTransform.GetComponent<Rigidbody>();
            segmentJoint.autoConfigureConnectedAnchor = false;

            if (isCloseConnected)
            {
                segmentJoint.connectedAnchor = Vector3.forward * 0.1f;
            }
            else
                segmentJoint.connectedAnchor = Vector3.forward * (totalLength / segmentCount);


            segmentJoint.xMotion = ConfigurableJointMotion.Locked;
            segmentJoint.yMotion = ConfigurableJointMotion.Locked;
            segmentJoint.zMotion = ConfigurableJointMotion.Locked;

            segmentJoint.angularXMotion = ConfigurableJointMotion.Free;
            segmentJoint.angularYMotion = ConfigurableJointMotion.Free;
            segmentJoint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0;
            segmentJoint.angularZLimit = softJointLimit;

            JointDrive jointDrive = new JointDrive();
            jointDrive.positionDamper = 0;
            jointDrive.positionSpring = 0;
            segmentJoint.angularXDrive = jointDrive;
            segmentJoint.angularYZDrive = jointDrive;
        }
    }
    public void UpdateMesh()
    {
        GenerateVertices();
        mFilter.mesh.vertices = vertices;
    }
    private void GenerateMesh()
    {
        createTriangles = true;
        if (meshdata == null)
        {
            meshdata = new WireMeshData(sides, segmentCount + 1, false);
        }
        else
            meshdata.ResetMesh(sides, segmentCount + 1, false);


        GenerateIndicesMap();
        GenerateVertices();
        meshdata.ProcessMesh();
        mesh = meshdata.CreateMesh();

        mFilter.sharedMesh = mesh;

        createTriangles = false;

    }

    private void GenerateVertices()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            GenerateCircleVerticesAndTriangles(segments[i], i);
        }
    }

    private void GenerateCircleVerticesAndTriangles(Transform segmentTransform, int segmentIndex)
    {
        float angleDiff = 360 / sides;
        Quaternion diffRotation = Quaternion.FromToRotation(Vector3.forward, segmentTransform.forward);

        for (int sideIndex = 0; sideIndex < sides; sideIndex++)
        {
            float angleInRad = sideIndex * angleDiff * Mathf.Deg2Rad;
            float x = -1 * radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            Vector3 previousPoint = new Vector3(x, y, 0);
            Vector3 rotatedPoint = diffRotation * previousPoint;

            Vector3 vertex = segmentTransform.position + rotatedPoint;
            int vertArrayIndex = segmentIndex * sides + sideIndex;
            vertices[vertArrayIndex] = vertex;


            //bool check
            if (createTriangles)
            {
                meshdata.AddVertex(vertex, new(0, 0), vertArrayIndex);
                bool createThisTriangle = segmentIndex < segmentCount - 1;
                if (createThisTriangle)
                {
                    int currentIncrement = 1;
                    int a = vertexIndicesMap[segmentIndex, sideIndex];
                    int b = vertexIndicesMap[segmentIndex + currentIncrement, sideIndex];
                    int c = vertexIndicesMap[segmentIndex, sideIndex + currentIncrement];
                    int d = vertexIndicesMap[segmentIndex + currentIncrement, sideIndex + currentIncrement];
                    bool isLastGap = sideIndex == sides - 1;
                    if (isLastGap)
                    {
                        c = vertexIndicesMap[segmentIndex, 0];
                        d = vertexIndicesMap[segmentIndex + currentIncrement, 0];
                    }
                    meshdata.AddTriangles(a, d, c);
                    meshdata.AddTriangles(d, a, b);
                }
            }

        }
    }

    private void GenerateIndicesMap()
    {
        vertexIndicesMap = new int[segmentCount + 1, sides + 1];

        int meshVertexIndex = 0;

        for (int segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
        {
            for (int sideIndex = 0; sideIndex < sides; sideIndex++)
            {
                vertexIndicesMap[segmentIndex, sideIndex] = meshVertexIndex;
                meshVertexIndex++;
            }
        }
    }
    private void SetupCollisionAvoidance()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            SphereCollider collider = segments[i].GetComponent<SphereCollider>();

            if (i - 1 >= 0)
            {
                Physics.IgnoreCollision(collider, segments[i - 1].GetComponent<SphereCollider>(), true);
            }

            if (i + 1 < segments.Length)
            {
                Physics.IgnoreCollision(collider, segments[i + 1].GetComponent<SphereCollider>(), true);
            }
        }
    }

    private void DestroySegments()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            Destroy(segments[i].gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if (segments == null || vertices == null)
            return;
        for (int i = 0; i < segments.Length; i++)
        {
            Gizmos.DrawWireSphere(segments[i].position, 0.1f);
        }
        for (int y = 0; y < vertices.Length; y++)
        {
            Gizmos.DrawSphere(vertices[y], 0.06f);
        }
    }
}
