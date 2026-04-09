namespace Data.Model
{
    public partial class ProductionOrderDetail : BaseModel
    {
        public long? MaterialID { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialPartNumber { get; set; }
        public long? CategoryUnitID { get; set; }
        public string? CategoryUnitName { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public long? BOMID { get; set; }
        public string? BOMECN { get; set; }
        public string? BOMECNVersion { get; set; }
        public int? QuantitySNP { get; set; }
        public int? QuantityBox { get; set; }
        public int? Priority { get; set; }
        public int? Quantity00 { get; set; }
        public int? QuantityActual00 { get; set; }
        public int? QuantityGAP00 { get; set; }
        public DateTime? Date01 { get; set; }
        public string? Date01Name { get; set; }
        public long? CategoryVehicleID01 { get; set; }
        public string? CategoryVehicleName01 { get; set; }
        public int? Quantity01 { get; set; }
        public int? QuantityActual01 { get; set; }
        public int? QuantityGAP01 { get; set; }
        public DateTime? Date02 { get; set; }
        public string? Date02Name { get; set; }
        public long? CategoryVehicleID02 { get; set; }
        public string? CategoryVehicleName02 { get; set; }
        public int? Quantity02 { get; set; }
        public int? QuantityActual02 { get; set; }
        public int? QuantityGAP02 { get; set; }
        public DateTime? Date03 { get; set; }
        public string? Date03Name { get; set; }
        public long? CategoryVehicleID03 { get; set; }
        public string? CategoryVehicleName03 { get; set; }
        public int? Quantity03 { get; set; }
        public int? QuantityActual03 { get; set; }
        public int? QuantityGAP03 { get; set; }
        public DateTime? Date04 { get; set; }
        public string? Date04Name { get; set; }
        public long? CategoryVehicleID04 { get; set; }
        public string? CategoryVehicleName04 { get; set; }
        public int? Quantity04 { get; set; }
        public int? QuantityActual04 { get; set; }
        public int? QuantityGAP04 { get; set; }
        public DateTime? Date05 { get; set; }
        public string? Date05Name { get; set; }
        public long? CategoryVehicleID05 { get; set; }
        public string? CategoryVehicleName05 { get; set; }
        public int? Quantity05 { get; set; }
        public int? QuantityActual05 { get; set; }
        public int? QuantityGAP05 { get; set; }
        public DateTime? Date06 { get; set; }
        public string? Date06Name { get; set; }
        public long? CategoryVehicleID06 { get; set; }
        public string? CategoryVehicleName06 { get; set; }
        public int? Quantity06 { get; set; }
        public int? QuantityActual06 { get; set; }
        public int? QuantityGAP06 { get; set; }
        public DateTime? Date07 { get; set; }
        public string? Date07Name { get; set; }
        public long? CategoryVehicleID07 { get; set; }
        public string? CategoryVehicleName07 { get; set; }
        public int? Quantity07 { get; set; }
        public int? QuantityActual07 { get; set; }
        public int? QuantityGAP07 { get; set; }
        public DateTime? Date08 { get; set; }
        public string? Date08Name { get; set; }
        public long? CategoryVehicleID08 { get; set; }
        public string? CategoryVehicleName08 { get; set; }
        public int? Quantity08 { get; set; }
        public int? QuantityActual08 { get; set; }
        public int? QuantityGAP08 { get; set; }
        public DateTime? Date09 { get; set; }
        public string? Date09Name { get; set; }
        public long? CategoryVehicleID09 { get; set; }
        public string? CategoryVehicleName09 { get; set; }
        public int? Quantity09 { get; set; }
        public int? QuantityActual09 { get; set; }
        public int? QuantityGAP09 { get; set; }
        public DateTime? Date10 { get; set; }
        public string? Date10Name { get; set; }
        public long? CategoryVehicleID10 { get; set; }
        public string? CategoryVehicleName10 { get; set; }
        public int? Quantity10 { get; set; }
        public int? QuantityActual10 { get; set; }
        public int? QuantityGAP10 { get; set; }
        public DateTime? Date11 { get; set; }
        public string? Date11Name { get; set; }
        public long? CategoryVehicleID11 { get; set; }
        public string? CategoryVehicleName11 { get; set; }
        public int? Quantity11 { get; set; }
        public int? QuantityActual11 { get; set; }
        public int? QuantityGAP11 { get; set; }
        public DateTime? Date12 { get; set; }
        public string? Date12Name { get; set; }
        public long? CategoryVehicleID12 { get; set; }
        public string? CategoryVehicleName12 { get; set; }
        public int? Quantity12 { get; set; }
        public int? QuantityActual12 { get; set; }
        public int? QuantityGAP12 { get; set; }
        public DateTime? Date13 { get; set; }
        public string? Date13Name { get; set; }
        public long? CategoryVehicleID13 { get; set; }
        public string? CategoryVehicleName13 { get; set; }
        public int? Quantity13 { get; set; }
        public int? QuantityActual13 { get; set; }
        public int? QuantityGAP13 { get; set; }
        public DateTime? Date14 { get; set; }
        public string? Date14Name { get; set; }
        public long? CategoryVehicleID14 { get; set; }
        public string? CategoryVehicleName14 { get; set; }
        public int? Quantity14 { get; set; }
        public int? QuantityActual14 { get; set; }
        public int? QuantityGAP14 { get; set; }
        public DateTime? Date15 { get; set; }
        public string? Date15Name { get; set; }
        public long? CategoryVehicleID15 { get; set; }
        public string? CategoryVehicleName15 { get; set; }
        public int? Quantity15 { get; set; }
        public int? QuantityActual15 { get; set; }
        public int? QuantityGAP15 { get; set; }

        public ProductionOrderDetail()
        {
            Quantity01 = 0;
            Quantity02 = 0;
            Quantity03 = 0;
            Quantity04 = 0;
            Quantity05 = 0;
            Quantity06 = 0;
            Quantity07 = 0;
            Quantity08 = 0;
            Quantity09 = 0;
            Quantity10 = 0;
            Quantity11 = 0;
            Quantity12 = 0;
            Quantity13 = 0;
            Quantity14 = 0;
            Quantity15 = 0;            
        }
    }
}

