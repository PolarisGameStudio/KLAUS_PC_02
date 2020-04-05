using UnityEngine;
using System.Collections;


public class SimbolSetter : MonoBehaviour
{

    [Range(1, 5)]
    public int TypeOfSimbol = 1;
    public Animator simbol;
    Animator newSimbol;
    public Transform spotTargetSimbol;
    //public int SortingOrder = 0;
    //public string SortingLayer;

    public void SetSimbols(int SortingOrder, string SortingLayer)
    {
        //creo el simbolo y lo pongo a la plataforma
        newSimbol = Instantiate(simbol, spotTargetSimbol.position, spotTargetSimbol.rotation) as Animator;
        newSimbol.transform.parent = spotTargetSimbol;
        SpriteRenderer renderer = newSimbol.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = SortingLayer;
        renderer.sortingOrder = SortingOrder;
        //Aqui Falta setear el tipo
        simbol.SetBool("Simbol_" + TypeOfSimbol, true);
        newSimbol.SetBool("Simbol_" + TypeOfSimbol, true);
    }

    public void ActiveSimbol()
    {
        newSimbol.SetBool("Active", true);
    }

    public void DeActiveSimbol()
    {
        newSimbol.SetBool("Active", false);
    }
}
