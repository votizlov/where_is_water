using FreeDraw;
using UnityEngine;
using UnityEngine.UI;

public class DrawingScript : MonoBehaviour
{
    public Sprite sprite;
    private Texture2D texture;
    private SpriteRenderer _spriteRenderer;
    private DrawingSettings _drawingSettings;

    void Start()
    {
        _drawingSettings = GetComponent<DrawingSettings>();
        _drawingSettings.SetEraser();
        _drawingSettings.SetMarkerWidth(25);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        texture = sprite.texture;
    }

    

    void DrawAtPosition(Vector2 position)
    {
        Debug.Log("draw at position");
        // Get brush texture and size
        Vector2 brushSize = new Vector2(4,4);
        texture = _spriteRenderer.sprite.texture;

        // Calculate the pixel coordinates of the brush
        int x = (int)(position.x - brushSize.x / 2);
        int y = (int)(position.y - brushSize.y / 2);
        int width = (int)brushSize.x;
        int height = (int)brushSize.y;

        // Draw brush texture on sprite texture
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Color brushColor = new Color(0, 0, 0, 0);//brushTexture.GetPixel(i, j);
                texture.SetPixel(x + i, y + j, brushColor);
            }
        }

        // Apply the changes to the texture
        texture.Apply();
        _spriteRenderer.sprite = Sprite.Create(texture,_spriteRenderer.sprite.rect, _spriteRenderer.sprite.pivot); //texture;
    }
}
