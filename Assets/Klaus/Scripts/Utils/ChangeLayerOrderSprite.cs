using UnityEngine;
using System.Collections;
[RequireComponent (typeof(SpriteRenderer))]
public class ChangeLayerOrderSprite : MonoBehaviour {

    protected SpriteRenderer _sprite;
    public SpriteRenderer SpriteR {
        get {
            if (_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();
            return _sprite;
        }
    }
   
    public void SetOrder(int i) {
        SpriteR.sortingOrder = i;
    }

    public void SetOrderName(string nameL) {
        SpriteR.sortingLayerName = nameL;
    }
}
