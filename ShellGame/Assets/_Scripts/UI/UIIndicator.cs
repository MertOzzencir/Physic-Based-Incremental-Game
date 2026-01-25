using Unity.VisualScripting;
using UnityEngine;

public class UIIndicator : MonoBehaviour
{
    [SerializeField] private Sprite[] indicators;
    [SerializeField] private UnityEngine.UI.Image UIImage;

    private Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        UIImage.transform.position = Input.mousePosition;
    }
    public void SetIndicator(CursorIndicator indicator)
    {
        UIImage.sprite = indicators[(int)indicator];
    }
    public void BreakCursorInitiaze()
    {
        anim.SetTrigger("canBreak");
    }
    public void BreakCursorFinish()
    {
        anim.SetTrigger("canFinishBreak");
    }
}

public enum CursorIndicator
{
    IdleMode = 0,
    BreakMode = 1,
    FailMode = 2
}