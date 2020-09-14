﻿using System;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class ReceiveDetail : IEntity
    {
        public int Id { get; set; }        
        public int Quantity { get; set; }
        public int PreviousQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalQuantity { get; set; }
        public string Remark { get; set; }      
        public Nullable<DateTime> ExpiryDate { get; set; }
        public virtual Receive Receive { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        public virtual ItemSubCategory ItemSubCategory { get; set; }
        public virtual Item Item { get; set; }
        public virtual Store Store { get; set; }
        public virtual Section Section { get; set; }
        public virtual Shelf Shelf { get; set; }
    }
}
