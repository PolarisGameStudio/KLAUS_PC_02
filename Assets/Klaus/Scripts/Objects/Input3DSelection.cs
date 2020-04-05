using UnityEngine;

public class Input3DSelection : MonoBehaviour
{
    [SerializeField]
    Renderer m_spriteInput;
    public Renderer SpriteForInput { get { return m_spriteInput; } }

    public virtual Vector3 Center
    {
        get
        {
            return transform.position;
        }
    }
    public virtual void SelectByInput()
    {
        Debug.Log("Touch Finger Print");
    }
}
