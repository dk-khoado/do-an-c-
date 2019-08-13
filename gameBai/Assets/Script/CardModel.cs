using UnityEngine;
using System;
[CreateAssetMenu(fileName = "Card Game", menuName = "Card/Card Uno")]
[Serializable]
public class CardModel : ScriptableObject
{    
    public int number;
    public Color_Card color;
    public bool isChucNang;
    public Sprite image;
    public Skill skill;
    public int amounDraw;
}
public enum Color_Card
{
    Red, Blue, Green, Yellow, Black
}
public enum Skill
{
    Draw, Wild, Reverse, Skip
}
