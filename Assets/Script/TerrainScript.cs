using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class TerrainScript : MonoBehaviour
{
    private Texture2D CloneTerrain;
    private SpriteRenderer SpriteRenderer;
    private PolygonCollider2D PolygonCollider2D;
    private float worldVsPixelWidthRatio;
    private float worldVsPixelHeightRatio;
    private int Size;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CloneTerrain = Instantiate(SpriteRenderer.sprite.texture);

        worldVsPixelWidthRatio = CloneTerrain.width / SpriteRenderer.bounds.size.x;
        worldVsPixelHeightRatio = CloneTerrain.height / SpriteRenderer.bounds.size.y;

        PolygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        GetComponent<PolygonCollider2D>().autoTiling = true;
    }

    private void Update()
    {
        
    }
    private void UpdateTexture()
    {
        SpriteRenderer.sprite = Sprite.Create(CloneTerrain,
                                        new Rect(0, 0, CloneTerrain.width, CloneTerrain.height),
                                        new Vector2(0.5f, 0.5f),
                                        worldVsPixelWidthRatio);
    }
    
    private bool CheckIfNothing()
    {
        foreach (var pixel in CloneTerrain.GetPixels())
            if (pixel.a != 0)
                return false;
        return true;
    }
    public void MakeAHole(float blastRadius, Vector2 bulletCenter)
    {
        bool setPixel(NativeArray<Color32> array, int width, int x, int y, Color32 value)
        {
            int index = y * width + x;
            if (x < 0 || y < 0 || x >= width || index >= array.Length)
                return false;
            array[index] = value;
            return true;
        }

        int roundblastRadius = (int)(blastRadius * worldVsPixelWidthRatio);
        Vector2 terrainCenter = transform.position;
        Vector2Int impactPoint = Vector2Int.CeilToInt(
            new Vector2((bulletCenter.x - terrainCenter.x) * worldVsPixelWidthRatio + CloneTerrain.width / 2,
                        (bulletCenter.y - terrainCenter.y) * worldVsPixelHeightRatio + CloneTerrain.height / 2));

        var terrainPixels = CloneTerrain.GetRawTextureData<Color32>();

        for(int i = 1 - roundblastRadius; i < roundblastRadius; i++)
        {
            setPixel(terrainPixels, CloneTerrain.width, impactPoint.x, impactPoint.y + i, Color.clear);
            int range = (int)Mathf.Sqrt(roundblastRadius * roundblastRadius - i * i);
            for(int j = 1; j < range; j++)
            {
                setPixel(terrainPixels, CloneTerrain.width, impactPoint.x + j, impactPoint.y + i, Color.clear);
                setPixel(terrainPixels, CloneTerrain.width, impactPoint.x - j, impactPoint.y + i, Color.clear);
            }
        }

        //CloneTerrain.SetPixels32(terrainPixels);
        CloneTerrain.Apply();
        UpdateTexture();

        Destroy(GetComponent<PolygonCollider2D>(),0);

        if (!CheckIfNothing() && GetComponents<PolygonCollider2D>().Length < 2)
            PolygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
    }
    
}
