using UnityEngine;
using UnityEngine.UI;

public class ResourceImage : MonoBehaviour
{
    public Image _image;

    public void Construct(Texture2D texture)
    {
        _image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    public void Deinitialize()
    {
        _image.sprite = null;
    }
}