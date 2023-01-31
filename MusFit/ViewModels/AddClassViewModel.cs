using MusFit.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContainerTest.ViewModels
{
    public class AddClassViewModel
    {

        [Display(Name ="課程名稱")]
		[Required(ErrorMessage = "必須填入課程名稱")]
        [StringLength(10, ErrorMessage = "最多輸入10個字")]
        public string cName { get; set; }

		[Display(Name = "課程項目")]
		[Required(ErrorMessage = "必須填入課程項目")]

		public int lcID { get; set; }

		[Display(Name = "課程時段")]
		[Required(ErrorMessage = "必須填入課程時段")]

		public int tID { get; set; }


		[Display(Name = "堂數")]
		[Required(ErrorMessage = "必須填入堂數")]
		[Range(5, 15, ErrorMessage = "堂數必須介於5 ~ 15")]
        public int cTotalLession { get; set; }

		[Display(Name = "教練")]
		[Required(ErrorMessage = "必須填入授課教練")]
		public int eID { get; set; }


		[Display(Name = "教室")]
		[Required(ErrorMessage = "必須填入課程教室")]

		public int roomID { get; set; }

		[Display(Name = "預計人數")]
		[Range(5,30,ErrorMessage = "預計人數必須介於5 ~ 30")]

		public int cExcept { get; set; }

		[Display(Name = "價錢")]
        [Range(1000, 20000, ErrorMessage = "價錢必須介於1000 ~ 20000")]
        public int cprice { get; set; }

		[Display(Name = "開課日")]
		[DataType(DataType.Date)]
		public DateTime CtDate { get; set; }

	}
}
