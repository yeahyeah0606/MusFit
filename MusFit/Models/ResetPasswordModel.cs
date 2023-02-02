using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MusFit.Models
{
    public class ResetPasswordModel
    {
        [Display(Name = "新密碼：")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "您必須輸入新密碼！")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "會員密碼的長度需再6~20個字元內！")]
        [RegularExpression(@"[a-zA-Z]+[a-zA-Z0-9]*$", ErrorMessage = "密碼僅能有英文或數字，且開頭需為英文字母！")]
        public string NewPassword { get; set; }

        [Display(Name = "確認密碼：")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請您再次輸入密碼！")]
        public string CheckPassword { get; set; }
    }
}

