using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject[] Tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        for (int i = 0; i < 15; i++)
        {
            int randomTile = Random.Range(0, Tiles.Length);
            int randomNumber = Random.Range(0, 40) * 50;
            Vector3 newScale = new Vector3(100, 100, randomNumber);
            Tiles[randomTile].transform.localScale = newScale;

            Vector3 newPosition = new Vector3(Tiles[randomTile].transform.position.x, Tiles[randomTile].transform.localScale.z / 1166.940397f, Tiles[randomTile].transform.position.z);
            Tiles[randomTile].transform.position = newPosition;

            //divide height by this 1158.940397
        }
    }
}
