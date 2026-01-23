using Unity.VisualScripting;
using UnityEngine;

public class UIndicator : MonoBehaviour
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
    public void SetIndicator(int index)
    {
        UIImage.sprite = indicators[index];
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
