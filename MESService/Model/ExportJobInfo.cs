using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESService.Model
{

    //class này dùng để lưu thông tin về tiến trình export, có thể lưu vào cache hoặc database để theo dõi
    public class ExportJobInfo
    {
        public string JobId { get; set; }
        public string Status { get; set; } // Processing, Completed, Failed
        public int Progress { get; set; } // 0-100
        public int CurrentRow { get; set; } // Dòng đang xử lý
        public int TotalRows { get; set; } // Tổng số dòng
        public string CurrentAction { get; set; } // Hành động đang thực hiện
        public string DownloadUrl { get; set; }
        public string FilePath { get; set; }
        public string Error { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? CompletedAt { get; set; }
        public BaseParameter Parameters { get; set; }

        public ExportJobInfo()
        {
            JobId = Guid.NewGuid().ToString();
            Status = "Pending";
            Progress = 0;
            CurrentRow = 0;
            TotalRows = 0;
            CreatedAt = DateTime.Now;
        }

        // Tính phần trăm hoàn thành dựa trên số dòng
        public int CalculateProgress()
        {
            if (TotalRows == 0) return Progress;
            return (int)((double)CurrentRow / TotalRows * 100);
        }

        // Lấy message chi tiết
        public string GetDetailMessage()
        {
            if (Status == "Completed") return "Hoàn tất!";
            if (Status == "Failed") return $"Lỗi: {Error}";

            if (TotalRows > 0 && CurrentRow > 0)
            {
                return $"{CurrentAction} - {CurrentRow}/{TotalRows} dòng ({CalculateProgress()}%)";
            }

            return $"{CurrentAction} - {Progress}%";
        }
    }
}
