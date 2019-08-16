using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Card Game", menuName = "Card/Card 13")]
public class CardModel13 : ScriptableObject
{
    public int number;
    public Chat_bai chat_Bai;
    public Ki_Hieu kihieu;
    public Sprite image;
}
public enum Ki_Hieu
{
    Nothing,J,Q,K,A
}
public enum Chat_bai
{
   Co = 0, Ro = 1, chuon= 2, bich=3
}
