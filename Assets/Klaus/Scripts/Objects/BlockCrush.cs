using UnityEngine;
using System.Collections;

public class BlockCrush : MonoBehaviour, ICrushObject
{
    public Animator anim;
    public float TimeAnimDestroy = 0.4f;
    bool isDestroyed = false;
    public GameObject crushSFX;
    public float ShakeTime = 0.5f;
    public int ShakePreset = 0;

    void OnEnable()
    {
        isDestroyed = false;
        anim.Rebind();
    }

    public void Crush(TypeCrush type = TypeCrush.Middle)
    {
        if (!isDestroyed)
        {
            if (SaveManager.Instance.dataKlaus != null)
            {
                if (type == TypeCrush.Upper)
                    SaveManager.Instance.dataKlaus.AddDestroy_ObjectUpper();
                SaveManager.Instance.dataKlaus.AddDestroy_Object();


            }
            StartDestroy();
        }
    }

    protected virtual void StartDestroy()
    {
        if (CameraShake.Instance != null)
            CameraShake.Instance.StartShakeBy(ShakeTime, ShakePreset);
        ManagerStop.Instance.StopAll(0.1f);

        isDestroyed = true;
        anim.SetBool("DestroyTrue", true);
        if (crushSFX != null)
            crushSFX.Spawn(transform.position, transform.rotation);
        Invoke("DestroyBox", TimeAnimDestroy);
        //Invoke("Sfx", TimeAnimDestroy);

    }

    protected virtual void DestroyBox()
    {
        gameObject.SetActive(false);
    }

    public void Sfx()
    {
        crushSFX.Spawn(transform.position, transform.rotation);
    }

}
