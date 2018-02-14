using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {
    [Header("Object with colors to choose from")]
    [Tooltip("Leve blank if script is sitting on object with texture.")] 
    [SerializeField] private RectTransform texture;
    private Texture2D myTexture;

    [Header("Circle to drag")]
    [Tooltip("Leave blank if you do not want have a draggable circle")]
    [SerializeField] RectTransform circle;
    [SerializeField] private Vector2 circleOffsetMouse;  //if circle does not sit perfectly around the mouse - adjust this
    [SerializeField] private Vector2 circleOffsetBounds; //if circle does not perfectly reach corners of a texture - adjust this

    private Color c;

    private void Start() {
        if (texture == null) {
            texture = GetComponent<RectTransform>();
            if (texture == null) {
                Debug.LogError("You forgot to assign texture");
                return;
            }
        }
        myTexture = texture.GetComponent<Image>().sprite.texture;

        circle.position = texture.transform.position;
    }

    public Color ReturnColor() {
        return c;
    }

    //Allows just pressing on a texture
    public void StartDragging() {
        Dragging();
    }

    public void Dragging() {
        Vector2 pos = texture.transform.InverseTransformPoint(Input.mousePosition);

        pos = new Vector2(pos.x + texture.rect.width / 2 / texture.localScale.x,
                          pos.y + texture.rect.height / 2 / texture.localScale.y);
        c = myTexture.GetPixel((int)pos.x, (int)pos.y);
        DragCircle();
    }

    private void DragCircle() {
        if (circle == null) {
            return;
        }
        circle.transform.position = new Vector2(Input.mousePosition.x - circleOffsetMouse.x,
                                                Input.mousePosition.y - circleOffsetMouse.y);

        Vector2 clampPos = circle.transform.position;
        clampPos.x = Mathf.Clamp(clampPos.x,
                        texture.transform.position.x - texture.rect.width * texture.parent.localScale.x / 2 + circle.rect.width / 2 - circleOffsetBounds.x,
                        texture.transform.position.x + texture.rect.width * texture.parent.localScale.x / 2 - circle.rect.width / 2 + circleOffsetBounds.x);
        clampPos.y = Mathf.Clamp(clampPos.y,
                        texture.transform.position.y - texture.rect.height * texture.parent.localScale.y / 2 + circle.rect.height / 2 - circleOffsetBounds.y,
                        texture.transform.position.y + texture.rect.height * texture.parent.localScale.y / 2 - circle.rect.height / 2 + circleOffsetBounds.y);

        circle.transform.position = clampPos;
    }
}