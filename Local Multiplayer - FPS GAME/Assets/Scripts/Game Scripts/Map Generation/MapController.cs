using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour
{
    public GameObject[] Tiles;
    public GameObject SmokeVfx;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int i = 0; i < 45; i++)
        {
            int randomTile = Random.Range(0, Tiles.Length);
            int randomNumber = Random.Range(1, 4) * 20;
            Vector3 newScale = new Vector3(100, 100, randomNumber);

            Vector3 newPosition = new Vector3(
                Tiles[randomTile].transform.position.x,
                Tiles[randomTile].transform.localScale.z * 0.0397325f,
                Tiles[randomTile].transform.position.z
            );

            StartCoroutine(MoveTile(Tiles[randomTile], newPosition, newScale));
            StartCoroutine(SpawnSmoke(new Vector3(Tiles[randomTile].transform.position.x, 0, Tiles[randomTile].transform.position.z)));
        }
    }

    IEnumerator MoveTile(GameObject tile, Vector3 targetPosition, Vector3 targetScale)
    {
        float duration = 5f; // Time in seconds
        float elapsedTime = 0;

        Vector3 startPosition = tile.transform.position;
        Vector3 startScale = tile.transform.localScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            tile.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        // Ensure final values are set precisely
        tile.transform.position = targetPosition;
        tile.transform.localScale = targetScale;
    }

    IEnumerator SpawnSmoke(Vector3 position)
    {
        yield return new WaitForSeconds(1f);
        Instantiate(SmokeVfx, position, Quaternion.identity);
    }
}
