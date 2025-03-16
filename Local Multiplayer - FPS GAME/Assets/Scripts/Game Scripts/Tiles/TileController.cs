using UnityEngine;
using UnityEngine.InputSystem;

public class TileController : MonoBehaviour
{
    public GameObject TestTile;
    public float moveSpeed = 5f;

    private Vector3 targetScale;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float countdown = 2f;

    public float raycastDistance;
    public Material HoverMaterial;
    public Material StandardMaterial;

    private Renderer HitTile;
    private Renderer PrevHitObject;

    public GameObject Camera;
    public GameObject SmokeVfx;
    public PlayerInput PlayerInput;
    
    
    private Color originalColor;
    private bool isHovering = false;

    public GameObject ActiveTile;

    private void Start()
    {
        // Subscribe to input actions
        PlayerInput.actions["TileUp"].performed += ctx => MoveTile(true);  // tile up is the right trigger & C on KBM
        PlayerInput.actions["TileDown"].performed += ctx => MoveTile(false); // tile down is the left trigger & V on KBM
    }

    void Update()
    {
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                HitTile = hit.collider.gameObject.GetComponent<MeshRenderer>();

                if (HitTile != PrevHitObject)
                {
                    HitTile.material = HoverMaterial;
                    if (PrevHitObject != null) PrevHitObject.material = StandardMaterial;
                    PrevHitObject = HitTile;
                }
            }
            else if (PrevHitObject != null)
            {
                PrevHitObject.material = StandardMaterial;
                PrevHitObject = null;
            }
        }
        
        if (isMoving)
        {
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                TestTile.transform.localScale = Vector3.Lerp(TestTile.transform.localScale, targetScale, moveSpeed * Time.deltaTime);
                TestTile.transform.position = Vector3.Lerp(TestTile.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                isMoving = false;
                countdown = 2f;
            }
        }
    }
    

    private void MoveTile(bool moveUp)
    {
        if (isMoving || HitTile == null) return;

        isMoving = true;
        TestTile = HitTile.gameObject;

        targetScale = TestTile.transform.localScale;
        targetPosition = TestTile.transform.position;

        if (moveUp)
        {
            targetScale.z += 15;
        }
        else
        {
            targetScale.z -= 15;
        }

        targetPosition = new Vector3(TestTile.transform.position.x, targetScale.z * 0.0397325f, TestTile.transform.position.z);
        SpawnSmoke(new Vector3(TestTile.transform.position.x, 0, TestTile.transform.position.z));
    }

    private void SpawnSmoke(Vector3 position)
    {
        Instantiate(SmokeVfx, position, Quaternion.identity);
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            ActiveTile = col.gameObject;
        }
    }

    void OnCollisionLeave(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            ActiveTile = null;
        }
    }
}
