using UnityEngine;
using UnityEngine.UI;

public class ControllerCard : MonoBehaviour
{
    public CardModel Properties;
    public Sprite image;
    private bool hover;
    public bool isSelect;
    Animator animator;
    // Start is called before the first frame update    
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (GetComponentInParent<ControllerPlayer>().isLocalPlayer)
        {
            if (!image)
            {
                image = Properties.image;
                GetComponent<Image>().sprite = image;
            }
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!hover)
            {
                isSelect = false;
                animator.SetBool("select", false);
                if (!GetComponentInParent<ControllerPlayer>().isLocalPlayer)
                {
                    return;
                }
                GameObject manager = GetComponentInParent<ControllerPlayer>().manager;
                //manager.GetComponent<ManagerGame>().currentCard = null;
            }
        }
    }
    public void SelectCard()
    {
        if (!GetComponentInParent<ControllerPlayer>().isLocalPlayer)
        {
            return;
        }
        GameObject manager = GetComponentInParent<ControllerPlayer>().manager;
        manager.GetComponent<ManagerGame>().selectCard = gameObject;
        isSelect = true;
        animator.SetBool("select", true);
    }
    public void HoverCard()
    {
        hover = true;
    }
    public void UnhoverCard()
    {
        hover = false;
    }
    public void ActiveSkill()
    {
        if (Properties.isChucNang)
        {
            switch (Properties.skill)
            {
                case Skill.Draw:
                    break;
                case Skill.Wild:

                    break;
                case Skill.Reverse:
                    break;
                case Skill.Skip:
                    break;
                default:
                    break;
            }
        }
    }
}
