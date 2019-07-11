using System;

namespace KeyRandom
{
    public class RanDomKey
    {
        public int RanInt()
        {
            Random random = new Random();
            int rdNum = random.Next(0, 9);
            return rdNum;
        }
        //Chữ Hoa
        public string RanUppercase()
        {
            Random rd = new Random();

            return Convert.ToChar(rd.Next(65, 90)).ToString();

        }
        //Chữ Thường
        public string RanLowerCase()
        {
            Random rd = new Random();

            return Convert.ToChar(rd.Next(97, 122)).ToString();
        }

        
    }
}

