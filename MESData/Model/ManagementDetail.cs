using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESData.Model
{
    public class ManagementDetail
    {
        // Thông tin cơ bản
        public string? LEAD_PN { get; set; }
        public string? DSCN_YN { get; set; } // 'Y' or 'N'
        public int? LEAD_INDEX { get; set; } // Có thể là int? hoặc long? tùy kiểu DB
        public string? HOOK_RACK { get; set; }
        public decimal? Safety_Stock { get; set; } // Dùng decimal? hoặc int? tùy kiểu DB
        public string? LEAD_SCN { get; set; } // "LEAD" or "SPST"

        // Thông tin Wire
        public int? W_PN_IDX { get; set; } // Có thể giữ IDX hoặc chỉ cần P/N
        public string? WIRE_PNO { get; set; } // Tương ứng TextBox8/Tab5_wirePNo (P/N thực tế)
        public string? WIRE_NM { get; set; } // Tương ứng TextBox24/Tab5_wireName
        public string? W_Diameter { get; set; } // Tương ứng TextBox25/Tab5_diameter
        public string? W_LINK { get; set; } // Tương ứng TextBox22/Tab5_wLink
        public string? W_Color { get; set; } // Tương ứng TextBox26/Tab5_color
        public string? WR_NO { get; set; } // Tương ứng TextBox23/Tab5_wrNo
        public decimal? W_Length { get; set; } // Tương ứng TextBox27/Tab5_length (dùng decimal?)

        // Thông tin Term 1
        public int? T1_PN_IDX { get; set; }
        public string? TERM1_PNO { get; set; } // Tương ứng TextBox9/Tab5_term1
        public int? S1_PN_IDX { get; set; }
        public string? SEAL1_PNO { get; set; } // Tương ứng TextBox10/Tab5_seal1
        public decimal? STRIP1 { get; set; } // Tương ứng TextBox11/Tab5_strip1 (dùng decimal?)
        public string? T1NO { get; set; } // Tương ứng TextBox20/Tab5_t1No
        public string? CCH_W1 { get; set; } // Tương ứng TextBox12/Tab5_cchW1
        public string? ICH_W1 { get; set; } // Tương ứng TextBox13/Tab5_ichW1

        // Thông tin Term 2
        public int? T2_PN_IDX { get; set; }
        public string? TERM2_PNO { get; set; } // Tương ứng TextBox14/Tab5_term2
        public int? S2_PN_IDX { get; set; }
        public string? SEAL2_PNO { get; set; } // Tương ứng TextBox15/Tab5_seal2
        public decimal? STRIP2 { get; set; } // Tương ứng TextBox16/Tab5_strip2 (dùng decimal?)
        public string? T2NO { get; set; } // Tương ứng TextBox21/Tab5_t2No
        public string? CCH_W2 { get; set; } // Tương ứng TextBox17/Tab5_cchW2
        public string? ICH_W2 { get; set; } // Tương ứng TextBox18/Tab5_ichW2

        // Thông tin khác
        public decimal? BUNDLE_QTY { get; set; }

        public List<SubPartInfo>? SubParts { get; set; }
       


    }
    public class SubPartInfo // Class riêng cho sub-part info
    {
        public int? NO { get; set; }
        public string? LEAD_NO { get; set; } // Lead No của sub-part
        public string? S_LR { get; set; } // LH or RH
    }
}
