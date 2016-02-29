﻿namespace Aegis.Data.Repositories.Contracts.Classes
{
    public class GroupInfo
    {
        public string GroupName { get; set; }
        public int ExpOption { get; set; }
        public byte ItemPickupRule { get; set; }
        public byte ItemDivisionRule { get; set; }
    }
}