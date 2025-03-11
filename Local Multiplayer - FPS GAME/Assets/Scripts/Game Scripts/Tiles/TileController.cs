using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject TestTile;
    public float moveSpeed = 5f;  

    private Vector3 targetScale;
    private Vector3 targetPosition;

    private bool isMoving = false;

    public float countdown = 2f;

    public float raycastDistance;

    public Material HoverMaterial;

    public Material StandardMaterial;

    private Renderer HitTile;

    private Renderer PrevHitObject;

    public GameObject Camera;


    private void Start()
    {
        
    }

    void Update()
    {
        Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
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
                    if (PrevHitObject == null)
                    {
                        PrevHitObject = HitTile;
                    }
                    else
                    {
                        PrevHitObject.material = StandardMaterial;
                        PrevHitObject = HitTile;
                    }
                }                
            }
            else
            {
                PrevHitObject.material = StandardMaterial;
            }
        }

        if (Input.GetKey(KeyCode.C) && !isMoving)  
        {
            isMoving = true;
            TestTile = HitTile.gameObject;
            targetScale = TestTile.transform.localScale;
            targetPosition = TestTile.transform.position;
            targetScale.z = TestTile.transform.localScale.z + 15;
            targetPosition = new Vector3(TestTile.transform.position.x, targetScale.z * 0.0397325f, TestTile.transform.position.z);
            
        }
        
        if (Input.GetKey(KeyCode.V) && !isMoving)  
        {
            isMoving = true;
            TestTile = HitTile.gameObject;
            targetScale = TestTile.transform.localScale;
            targetPosition = TestTile.transform.position;
            targetScale.z = TestTile.transform.localScale.z - 15;
            targetPosition = new Vector3(TestTile.transform.position.x, targetScale.z / 0.0397325f, TestTile.transform.position.z);
            
        }

        if (isMoving)
        {
            if (countdown > 0)
            {
                countdown = countdown - Time.deltaTime;
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
}
