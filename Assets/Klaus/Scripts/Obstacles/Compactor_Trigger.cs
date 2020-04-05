using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Compactor_Trigger : MonoBehaviour {

    public Animator animatorTop;
    public Animator animatorDown;
    public Transform MiddleCompactor;
    float BaseScale = 0.1f;
    public Transform DownCompactor;
    float basePosT = 2.7f;
    [Tooltip("Maxima posicion en Y para la parte de abajo")]
    public float maxPosY = -1.5f;
    [Tooltip("Maxima escala en Y para la parte del medio")]
    public float maxScale = 1.0f;
    [Tooltip("Tiempo Que tarda en Llegar al MaxPosY")]
    public float TimeForDown = 1.0f;
    [Tooltip("Tiempo Que tarda en Llegar a la posicion inicial")]
    public float TimeForUp = 1.0f;
    [Tooltip("Tiempo Para antes de Bajar, este empieza a correr despues de la primera animacion")]
    public float TimeUpRest = 0.5f;
    [Tooltip("Tiempo Para antes de subir, este empieza a correr despues de la animaciond e hit.")]
    public float TimeDownRest = 0.5f;
    private Behaviour_Compactor behaviourTop;
    private Behaviour_Compactor_Down behaviourDown;
    bool isActive = false;


    Tweener twen;
    Tweener twenScale;

    void Start() {
        BaseScale = MiddleCompactor.localScale.y;
        basePosT = DownCompactor.localPosition.y;

        // Find a reference to the ExampleStateMachineBehaviour in Start since it might not exist yet in Awake.
        behaviourTop = animatorTop.GetBehaviour<Behaviour_Compactor>();
        behaviourDown = animatorDown.GetBehaviour<Behaviour_Compactor_Down>();

        // Set the StateMachineBehaviour's reference to an ExampleMonoBehaviour to this.
        behaviourTop.compactorT = this;
        behaviourDown.compactorT = this;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !isActive) {
            isActive = true;
            animatorTop.SetBool("Active", true);
        }

    }
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && !isActive) {
            isActive = true;
            animatorTop.SetBool("Active", true);
        }

    }
    public void Down() {
        StartCoroutine("TimeRestUp", TimeUpRest);

    }
    IEnumerator TimeRestUp(float time) {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        animatorDown.SetBool("Active", true);
        twen = DownCompactor.DOLocalMoveY(maxPosY, TimeForDown).OnComplete(Hit);
        twenScale = MiddleCompactor.DOScaleY(maxScale, TimeForDown);
    }
    void Hit() {
        animatorDown.SetTrigger("Hit");
    }

    public void Up() {
        StartCoroutine("TimeRestDown", TimeDownRest);
    }
    IEnumerator TimeRestDown(float time) {
        yield return StartCoroutine(new TimeCallBacks().WaitForSecondsPauseStop(time));
        animatorDown.SetBool("Active", false);
        twen = DownCompactor.DOLocalMoveY(basePosT, TimeForUp).OnComplete(Rest);
        twenScale = MiddleCompactor.DOScaleY(BaseScale, TimeForUp);
    }
    void Rest() {
        animatorTop.SetBool("Active", false);
        isActive = false;
    }

}
