namespace MESData.Model
{
    public class FAWorkOrderHistory
    {
        public int ID { get; set; }
        public int WorkOrderID { get; set; }
        public string PartNumber { get; set; }
        public DateTime OldDate { get; set; }
        public DateTime NewDate { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual FAWorkOrder WorkOrder { get; set; }
    }
}