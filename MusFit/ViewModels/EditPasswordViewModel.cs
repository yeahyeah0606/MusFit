using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MusFit.ViewModels
{
    public class EditPasswordViewModel
    {
        [Display(Name = "舊密碼：")]
        [DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        [Required(ErrorMessage = "您必須輸入舊密碼！")]
        [Remote("EditPassword", "Front", HttpMethod = "POST", ErrorMessage = "舊密碼輸入錯誤!")]
        public string OldPassword { get; set; }
        [Display(Name = "新密碼：")]
        [DataType(DataType.Password)]   //表示此欄位為密碼欄位，所以輸入時會產生隱碼
        [Required(ErrorMessage = "您必須輸入新密碼！")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "會員密碼的長度需再6~20個字元內！")]
        [RegularExpression(@"[a-zA-Z]+[a-zA-Z0-9]*$", ErrorMessage = "密碼僅能有英文或數字，且開頭需為英文字母！")]
        public string NewPassword { get; set; }
        [Display(Name = "確認會員密碼：")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請您再次輸入密碼！")]
        public string CheckPassword { get; set; }
    }
}
