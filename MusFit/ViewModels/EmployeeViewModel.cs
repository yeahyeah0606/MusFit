using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace MusFit.ViewModels
{
    public class EmployeeViewModel
    {
        public int EId { get; set; }

        public string ENumber { get; set; }

        [Display(Name = "真實姓名", Prompt = "ex.陳小美")]
        [Required(ErrorMessage = "您必須填入真實姓名")]
        [StringLength(5, ErrorMessage = "最多輸入五個中文字")]
        public string EName { get; set; }
        [Display(Name = "英文名字", Prompt = "ex.Jenny")]
        [Required(ErrorMessage = "您必須填入英文名字")]
        public string EEngName { get; set; }

        [Required(ErrorMessage = "您需要輸入電子信箱")]
        [Display(Name = "電子信箱", Prompt = "ex.ab123@gmail.com")]
        [EmailAddress(ErrorMessage = "只允許輸入Email格式")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "您需要選擇性別")]
        [Display(Name = "性別")]
        public bool? EGender { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "您需要選擇生日")]
        public DateTime? EBirth { get; set; }

        [Required(ErrorMessage = "您需要輸入手機號碼")]
        [Display(Name = "手機號碼", Prompt = "ex.0912345678")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機號碼格式!")]
        public string EPhone { get; set; }

        [Required(ErrorMessage = "您需要輸入帳號")]
        [Display(Name = "帳號", Prompt = "英文名字+生日 ex.Jenny0123")]
        public string EAccount { get; set; }
        public string EPassword { get; set; }
        [Required(ErrorMessage = "您需要輸入緊急聯絡人")]
        [Display(Name = "緊急聯絡人", Prompt = "ex.王大明")]
        [StringLength(50)]
        public string EContactor { get; set; }

        [Required(ErrorMessage = "您需要輸入緊急聯絡人電話")]
        [Display(Name = "緊急聯絡人電話", Prompt = "ex.0912345678")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^09[0-9]{8}$", ErrorMessage = "請輸入正確手機號碼格式!")]
        public string EContactorPhone { get; set; }
        public string EPhoto { get; set; }

        [Required(ErrorMessage = "您需要輸入地址")]
        [Display(Name = "地址", Prompt = "ex.台中市西屯區台灣大道2號")]
        [DataType(DataType.Text)]
        public string EAddress { get; set; }

        [Display(Name = "入職日期")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "您需要選擇入職日期")]
        public DateTime? EEnrollDate { get; set; }

        [Display(Name = "離職日期")]
        [DataType(DataType.DateTime)]
        public DateTime? EResignDate { get; set; }
        public string EToken { get; set; }

        [Required(ErrorMessage = "您需要選擇職位")]
        [Display(Name = "職位")]
        public bool? EIsCoach { get; set; }

        
        [Display(Name = "個人描述", Prompt = "描述自己....")]
        public string EExplain { get; set; }

        [Required(ErrorMessage = "您需要輸入身分證")]
        [Display(Name = "身分證", Prompt = "ex.A123456789")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[A-Z]{1}[0-9]{9}$", ErrorMessage = "請輸入正確身分證格式!")]
        public string EIdentityNumber { get; set; }
        public IList<string> SelectedLession { get; set; }

       
        public IList<SelectListItem> AvailableLession { get; set; }

        public EmployeeViewModel()
        {
            SelectedLession = new List<string>();
            AvailableLession = new List<SelectListItem>();
        }
    }
}
