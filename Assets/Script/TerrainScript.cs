using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    private Texture2D CloneTerrain;
    private SpriteRenderer SpriteRenderer;
    private float worldVsPixelWidthRatio;
    private float worldVsPixelHeightRatio;
    // Start is called before the first frame update
    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CloneTerrain = Instantiate(SpriteRenderer.sprite.texture);

        worldVsPixelWidthRatio = CloneTerrain.width / SpriteRenderer.bounds.size.x;
        worldVsPixelHeightRatio = CloneTerrain.height / SpriteRenderer.bounds.size.y;
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void UpdateTexture()
    {
        SpriteRenderer.sprite = Sprite.Create(CloneTerrain,
                                        new Rect(0, 0, CloneTerrain.width, CloneTerrain.height),
                                        new Vector2(0.5f, 0.5f),
                                        128);
    }
    private void ExploseBullet(Collider2D bulletCollider)
    {
        Vector2 bulletCenter = bulletCollider.bounds.center;
        Texture2D bulletShape = bulletCollider.GetComponent<BulletScript>().BulletShape;
        float bulletRadius = bulletCollider.GetComponent<BulletScript>().Radius;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(bulletCenter, bulletRadius);

        print(hitColliders.Length);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.tag != "Terrain")
                continue;
            hitCollider.GetComponent<TerrainScript>().MakeAHole(bulletShape, bulletCenter);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.GetComponent<BulletScript>().IsTrigged)
        {
            ExploseBullet(collider);
            Destroy(collider.gameObject);
            collider.GetComponent<BulletScript>().IsTrigged = true;
        }
    }
    public void MakeAHole(Texture2D bulletShape, Vector2 bulletCenter)
    {
        Vector2 terrainCenter = transform.position;
        Vector2Int impactPoint = 
            Vector2Int.CeilToInt(new Vector2((bulletCenter.x - terrainCenter.x) * worldVsPixelWidthRatio + CloneTerrain.width / 2,
                                             (bulletCenter.y - terrainCenter.y) * worldVsPixelHeightRatio + CloneTerrain.height / 2));

        int limLeft = Mathf.Max(0, impactPoint.x - bulletShape.width / 2 - 1);
        int limUp = Mathf.Max(0, impactPoint.y - bulletShape.height / 2 - 1);
        int limRight = Mathf.Min(impactPoint.x + bulletShape.width / 2 - 1, CloneTerrain.width - 1);
        int limDown = Mathf.Min(impactPoint.y + bulletShape.height / 2 - 1, CloneTerrain.height - 1);
        int resLeft = limLeft - (impactPoint.x - bulletShape.width / 2 - 1);
        int resUp = limUp - (impactPoint.y - bulletShape.height / 2 - 1);

        var bulletShapePixels = bulletShape.GetPixels();
        var terrainPixels = CloneTerrain.GetPixels(limLeft, limUp, limRight - limLeft + 1, limDown - limUp + 1);

        for (int i = 0; i < limDown - limUp + 1; i++) 
        {
            for(int j = 0; j < limRight - limLeft + 1; j++)
            {
                if (bulletShapePixels[resLeft + j + (resUp + i) * bulletShape.width] == Color.black) 
                    terrainPixels[j + i * (limRight - limLeft + 1)] = Color.clear;
            }
        }

        CloneTerrain.SetPixels(limLeft, limUp, limRight - limLeft + 1, limDown - limUp + 1, terrainPixels);
        CloneTerrain.Apply();
        UpdateTexture();
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }
    
}
