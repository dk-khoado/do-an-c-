using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Card Game",menuName = "Card/Card Uno")]
public class CardModel : ScriptableObject
{
    public int number;
    public Color_Card color;
    public bool isChucNang;
    public Sprite image;
    public Skill skill;
}
public enum Color_Card
{
    Red,Blue,Green,Yellow, Black
}
public enum Skill
{
    Draw, Wild, Reverse,Skip
}
