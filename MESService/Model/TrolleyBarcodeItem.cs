namespace MESService.Model
{
    public class TrolleyBarcodeItem
    {
        public string? BARCODE { get; set; }
        public string? LEAD_NO { get; set; }
        public string? HOOK_RACK { get; set; }
        public int QTY { get; set; }
    }

    public class TrolleySummaryItem
    {
        public string? LEAD_NO { get; set; }
        public string? HOOK_RACK { get; set; }
        public int BarcodeCount { get; set; }
        public int TotalQty { get; set; }
    }
}