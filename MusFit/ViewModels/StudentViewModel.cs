using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MusFit.ViewModels
{
    public class StudentViewModel
    {
        public int SId { get; set; }

        [Display(Name = "會員編號", Prompt = "ex.S0030")]
        [Required(ErrorMessage = "您需要輸入會員編號")]
        [RegularExpression(@"^S[0-9]{4}$", ErrorMessage = "請輸入正確會員編號格式!")]
        public string SNumber { get; set; }
        [Display(Name = "真實姓名", Prompt = "ex.陳小美")]
        [Required(ErrorMessage = "您必須填入真實姓名")]
        [StringLength(5, ErrorMessage = "最多輸入五個中文字")]
        public string SName { get; set; }

        [Required(ErrorMessage = "您需要輸入電子信箱")]
        [Display(Name = "電子信箱", Prompt = "ex.ab123@gmail.com")]
        [EmailAddress(ErrorMessage = "只允許輸入Email格式")]
        public string SMail { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "您需要輸入生日")]
        public DateTime? SBirth { get; set; }

        [Required(ErrorMessage = "您需要輸入性別")]
        [Display(Name = "性別")]
        public bool SGender { get; set; }

        [Required(ErrorMessage = "您需要輸入緊急聯絡人")]
        [Display(Name = "緊急聯絡人", Prompt = "ex.王大明")]
        [StringLength(50)]
        public string SContactor { get; set; }

        [Required(ErrorMessage = "您需要輸入緊急聯絡人電話")]
        [Display(Name = "緊急聯絡人電話", Prompt = "ex.0912345678")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機號碼格式!")]
        public string SContactPhone { get; set; }
        public string SPhoto { get; set; }

        [Required(ErrorMessage = "您需要輸入地址")]
        [Display(Name = "地址", Prompt = "ex.台中市西屯區台灣大道2號")]
        [DataType(DataType.Text)]
        public string SAddress { get; set; }

        [Required(ErrorMessage = "您需要輸入手機號碼")]
        [Display(Name = "手機號碼", Prompt = "ex.0914725836")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機號碼格式!")]
        public string SPhone { get; set; }

        [Required(ErrorMessage = "您需要輸入帳號")]
        [Display(Name = "帳號", Prompt = "取Email帳號 ex.ab123")]
        public string SAccount { get; set; }
        public string SPassword { get; set; }
        public string SToken { get; set; }

        [Display(Name = "入會日期")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "您需要輸入入會日期")]
        public DateTime? SJoinDate { get; set; }

        public bool? SIsStudentOrNot { get; set; }
    }
}
