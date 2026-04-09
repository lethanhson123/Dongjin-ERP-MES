using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESData.Model
{
    public class ROTestLog : BaseModel
    {
        public long ID { get; set; }                
        public DateTime DateTime { get; set; }      

        public string? PartNumber { get; set; }     
        public string? PartName { get; set; }        
        public string? LotNum { get; set; }  
        public string? LineNumber { get; set; }  
        public string? LineName { get; set; }     
        public string? Remark { get; set; } 

        public string? ALC { get; set; }  
        public string? Retest { get; set; } 
        public string? ECO { get; set; }  
        public string? VER { get; set; }  
        public string? ETC { get; set; } 
        public string? ProgramVersion { get; set; } 

        public string? PassCount { get; set; }  
        public string? ScanBarcode { get; set; }
        public bool? Active { get; set; }
        public string? Note { get; set; }
        public DateTime? Update_DTM { get; set; }
        public string? Update_User { get; set; }

        public ROTestLog()
        {
        }
    }
}
