using UnityEngine;
public enum TypeCrush
{
    Upper,
    Down,
    Middle,
    NONE
}
public interface ICrushObject
{
    void Crush(TypeCrush type = TypeCrush.Middle);

}