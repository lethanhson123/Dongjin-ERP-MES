using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESService.Model
{
    public class D01Model: MESData.Model.BaseModel
    {
        public string PO_CODE { get; set; }     
        public string PO_DATE { get; set; }  
        public string PART_NO { get; set; }   
        public string PART_NAME { get; set; }  
        public string MODEL { get; set; }     
        public string GROUP { get; set; } 
        public int PART_SNP { get; set; }   
        public int STOCK { get; set; }   
        public int PO_QTY { get; set; }  
        public int Sales_QTY { get; set; }  
        public decimal COST { get; set; }            
        public decimal Sales { get; set; }        
        public int P_COUNT { get; set; }
        public int ACT_QTY { get; set; }
        public decimal RAT { get; set; }  

        public int PO_01 { get; set; }
        public int ACT_01 { get; set; }
        public decimal Sales_01 { get; set; }

        public int PO_02 { get; set; }
        public int ACT_02 { get; set; }
        public decimal Sales_02 { get; set; }

        public int PO_03 { get; set; }
        public int ACT_03 { get; set; }
        public decimal Sales_03 { get; set; }

        public int PO_04 { get; set; }
        public int ACT_04 { get; set; }
        public decimal Sales_04 { get; set; }

        public int PO_05 { get; set; }
        public int ACT_05 { get; set; }
        public decimal Sales_05 { get; set; }

        public int PO_06 { get; set; }
        public int ACT_06 { get; set; }
        public decimal Sales_06 { get; set; }

        public int PO_07 { get; set; }
        public int ACT_07 { get; set; }
        public decimal Sales_07 { get; set; }

        public int PO_08 { get; set; }
        public int ACT_08 { get; set; }
        public decimal Sales_08 { get; set; }

        public int PO_09 { get; set; }
        public int ACT_09 { get; set; }
        public decimal Sales_09 { get; set; }

        public int PO_10 { get; set; }
        public int ACT_10 { get; set; }
        public decimal Sales_10 { get; set; }

        public int PO_11 { get; set; }
        public int ACT_11 { get; set; }
        public decimal Sales_11 { get; set; }

        public int PO_12 { get; set; }
        public int ACT_12 { get; set; }
        public decimal Sales_12 { get; set; }
    }
}
