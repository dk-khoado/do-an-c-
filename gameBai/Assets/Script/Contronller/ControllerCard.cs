using UnityEngine;
using UnityEngine.UI;

public class ControllerCard : MonoBehaviour
{
    public int id;
    public CardModel Properties;
    public CardModel13 Properties_V13;
    public Sprite image;
    private bool hover;
    public bool isSelect;
    public Animator animator;
    // Start is called before the first frame update    
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (GetComponentInParent<ControllerPlayer>().isLocalPlayer)
        {
            if (!image)
            {
                if (Properties)
                {
                    image = Properties.image;
                    GetComponent<Image>().sprite = image;
                }
                else if (Properties_V13)
                {
                    image = Properties_V13.image;
                    GetComponent<Image>().sprite = image;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (GetComponentInParent<ControllerPlayer>().manager)
        {
            GameObject manager = GetComponentInParent<ControllerPlayer>().manager;
            if (manager.GetComponent<ManagerGame>())
            {
                if (gameObject.GetInstanceID() == manager.GetComponent<ManagerGame>().selectCard.GetInstanceID())
                {
                    Debug.Log(true);
                    animator.SetBool("select", true);
                }
                else
                {
                    Debug.Log(false);
                    animator.SetBool("select", false);
                }
            }
            else if (manager.GetComponent<ManagerGame_catte>())
            {
                if (manager.GetComponent<ManagerGame_catte>())
                {
                    if (manager.GetComponent<ManagerGame_catte>().selectCard)
                    {
                        if (gameObject.GetInstanceID() == manager.GetComponent<ManagerGame_catte>().selectCard.GetInstanceID())
                        {
                            animator.SetBool("select", true);
                        }
                        else
                        {
                          
                            animator.SetBool("select", false);
                        }
                    }
                    else
                    {
                       
                        animator.SetBool("select", false);
                    }                   
                }
            }

        }
    }
    public void SelectCard()
    {
        if (!GetComponentInParent<ControllerPlayer>().isLocalPlayer)
        {
            return;
        }
        if (Properties)
        {
            GameObject manager = GetComponentInParent<ControllerPlayer>().manager;
            manager.GetComponent<ManagerGame>().selectCard = gameObject;
            isSelect = true;
            animator.SetBool("select", true);
        }
        else if (Properties_V13)
        {
            GameObject manager = GetComponentInParent<ControllerPlayer>().manager;
            manager.GetComponent<ManagerGame_catte>().selectCard = gameObject;
            isSelect = true;
            animator.SetBool("select", true);
        }


    }
    public void HoverCard()
    {
        hover = true;
    }
    public void UnhoverCard()
    {
        hover = false;
    }
    public void DeSelect()
    {
        isSelect = false;
        animator.SetBool("select", false);
    }
}
