using System.ComponentModel.DataAnnotations;

namespace CRUD_Operation.Models
{
    public class ProductWebModel
    {
        public int ID { get; set; }
        [Display(Name = "Product")]
        [Required(ErrorMessage = "The Product field is required.")]
        public string? PRODUCT { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "The Description field is required.")]
        public string? DESCRIPTION { get; set; }
        [Display(Name = "Created Date")]
        public string? CREATEDDATE { get; set; }
    }
}
