using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card Game", menuName = "Card/CardUno")]
public class cardModel : ScriptableObject
{
    public int number;
    public Color_Card color;
    public Chucnang chucnang;
    public Image image;

    public enum Color_Card
    {
        Red, Blue, Green, Yellow, Black
    }
    public enum Chucnang
    {
        direction, prohibit, plus, discoloration, none
    }

}
