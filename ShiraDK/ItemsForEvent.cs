//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShiraRDKWork
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemsForEvent
    {
        public int Number { get; set; }
        public Nullable<int> ItemsID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> EventsID { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual Item Item { get; set; }
    }
}
