using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeed = 1.5f;
    private Renderer logRenderer;

    void Start()
    {
        logRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        logRenderer.material.mainTextureOffset = new Vector2(0, -offset);
    }
}
