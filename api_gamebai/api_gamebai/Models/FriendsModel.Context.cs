﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DatabaseGameBai_friend : DbContext
    {
        public DatabaseGameBai_friend()
            : base("name=DatabaseGameBai_friend")
        {
            vListFriends = Set<vListFriend>();
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<player_chatlist> player_chatlist { get; set; }
        public virtual DbSet<player_message> player_message { get; set; }
        internal virtual DbSet<vListFriend> vListFriends { get; set; }
        public virtual DbSet<player_listfriend> player_listfriend { get; set; }
    }
}