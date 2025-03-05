using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject TestTile;
    public float moveSpeed = 5f;  

    private Vector3 targetScale;
    private Vector3 targetPosition;

    private void Start()
    {
        targetScale = TestTile.transform.localScale;
        targetPosition = TestTile.transform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))  
        {
            targetScale.z = TestTile.transform.localScale.z + 250;

            targetPosition = new Vector3(TestTile.transform.position.x, targetScale.z / 1166.940397f, TestTile.transform.position.z);
        }

        TestTile.transform.localScale = Vector3.Lerp(TestTile.transform.localScale, targetScale, moveSpeed * Time.deltaTime);
        TestTile.transform.position = Vector3.Lerp(TestTile.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
