using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_gamebai.Models;

namespace api_gamebai.Controllers
{
    public class ItemManagerController : ApiController
    {
        DatabaseGameBai_Item db = new DatabaseGameBai_Item();

        [HttpPost]
        public ResponseMessage AddToShop([FromBody]shop_game shop)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Fail!!!");
            }
            if (shop == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Trống !!!");
            }

            if (shop.name_item == "" || shop.cost.ToString() == "" || shop.isEnable.ToString() == "" || shop.type_item == "" || shop.duration_day_.ToString() == null || shop.duration_day_.ToString() == "" )
            {
                return new ResponseMessage(BadRequest().ToString(), "Không đc nhập thiếu");
            }

            if (db.shop_game.Count(e => e.name_item == shop.name_item) > 0)
            {
                return new ResponseMessage("Tên skin đã tồn tại");
            }
            else
            {
                if (shop.type_item == "Card" || shop.type_item == "Table" || shop.type_item == "Avatar")
                {
                   /* //Coi như là vĩnh viễn luôn đi
                    do
                    {
                        shop.duration_day_ = 1000000;
                    } while (shop.duration_day_ == null || shop.duration_day_.ToString() == "");*/
                    db.AddToShop(shop.name_item, shop.cost, shop.isEnable, shop.type_item, shop.duration_day_);
                    return new ResponseMessage("Thêm skin " + shop.type_item + " thành công" );
                }
                else
                {
                    return new ResponseMessage("Sai loại skin, chỉ có 3 loại là: Card, Table, Avatar");
                }
            }
        }

        [HttpPost]
        public ResponseMessage BuySkin([FromBody] player_inventory player)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Fail!!!");
            }
            if (player == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Trống !!!");
            }
            if (player.player_id.ToString() == "" || player.item_id.ToString() == "")
            {
                return new ResponseMessage(BadRequest().ToString(), "Không đc nhập thiếu");
            }

            if (db.player_inventory.Count(e => e.item_id == player.item_id) > 0 && db.player_inventory.Count(e => e.player_id == player.player_id) > 0)
            {
                
                try
                {
                    //Renewal là gia hạn
                    //Nếu đã có skin cần mua rồi thì gia hạn
                    db.Renewal(player.player_id, player.item_id);
                    return new ResponseMessage("Gia hạn thành công", 1);
                }
                catch
                {
                    return new ResponseMessage("Không đủ tiền gia hạn món này", 0);
                }
            }
            else
            {
                if (db.shop_game.Count(e => e.id_item == player.item_id) > 0)
                {
                    try
                    {
                        //Mua skin
                        db.BuyInventory(player.player_id, player.item_id);
                        return new ResponseMessage("Mua thành công", 1);
                    }
                    catch
                    {
                        return new ResponseMessage("Không đủ tiền mua món này", 0);
                    }
                }
                else
                {
                    return new ResponseMessage("Thứ bạn mua không tồn tại", 0);
                }
            }



        }


        [HttpPost]
        public ResponseMessage SellSkin([FromBody] player_inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseMessage(BadRequest().ToString(), "Fail!!!", 0);
            }
            if (inventory == null)
            {
                return new ResponseMessage(BadRequest().ToString(), "Trống !!!", 0);
            }
            if(inventory.player_id.ToString() == "" || inventory.item_id.ToString() == "")
            {
                return new ResponseMessage(BadRequest().ToString(), "Không được nhập thiếu !!!", 0);
            }

            if (db.player_inventory.Count(e => e.item_id == inventory.item_id) > 0)
            {
                db.SellSkin(inventory.player_id, inventory.item_id);
                return new ResponseMessage(Ok().ToString(), "Bán thành công !!!", 1);
            }
            else
            {
                return new ResponseMessage(BadRequest().ToString(), "Item không tồn tại !!!", 0);
            }
        }
    }
}
