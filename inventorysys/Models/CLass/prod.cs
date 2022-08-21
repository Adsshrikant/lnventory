using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace inventorysys.Models
{
    [MetadataType(typeof(prodmetadata))]
    public partial class prod
    {

    }

    public class prodmetadata
    {
        [Display(Name = "Product Quantity")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Product Quantity")]
        public string item_SKU { get; set; }

        [Display(Name = "Product Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Product Description")]
        public string item_description { get; set; }

        [Display(Name = "Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Product Price")]
        public Nullable<decimal> item_price { get; set; }

        [Display(Name = "Availability")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Availabilty")]
        public Nullable<bool> item_Availability { get; set; }
    }
}