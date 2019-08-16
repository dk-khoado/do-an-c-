using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Models
{
    [Serializable]
    public class ShopItem
    {
        public int id_item;
        public string avatar;
        public string name_item;
        public double cost;
        public string descript;
    }

    [Serializable]
    public class ResponseShopItem
    {
        public List<ShopItem> data;
        public int result;
    }
    [Serializable]
    public class DataShop
    {
        public string response;
        public string message;
        public int result;
        public List<ShopItem> data;
    }

    
}
