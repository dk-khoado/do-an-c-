//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace api_gamebai.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class player_message
    {
        public int id_chatlist { get; set; }
        public string message { get; set; }
        public System.DateTime send_date { get; set; }
        public bool isdelete { get; set; }
        public int id { get; set; }
    
        public virtual player_chatlist player_chatlist { get; set; }
    }
}