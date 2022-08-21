using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace inventorysys.Models
{
    [MetadataType(typeof(clientmetadata))]

    public partial class client
    {

    }

    public class clientmetadata
    {
        [Display(Name ="Username")]
        [Required (AllowEmptyStrings =false,ErrorMessage ="Username Required")]
        public string c_username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        public string c_password { get; set; }

        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        public string c_email { get; set; }

        public string ResetPassLink { get; set; }



    }
}