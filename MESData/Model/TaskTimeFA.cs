using MESData.Model;

public class TaskTimeFA
{
    public long ID { get; set; }
    public long? LineID { get; set; }
    public string PartNo { get; set; }
    public string? ECN { get; set; }
    public decimal? TaskTimeIE { get; set; }
    public decimal? TaskTimeIE2 { get; set; }    
    public decimal? TaskTimeIE3 { get; set; }    
    public decimal? TaskTimeIE4 { get; set; }    
    public decimal? TaskTimeCus { get; set; }
    public bool Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? CreateUserName { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string? UpdateUserName { get; set; }
    public virtual LineList? Line { get; set; }
}