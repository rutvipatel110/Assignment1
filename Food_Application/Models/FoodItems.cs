using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Food_Application.Models
{
    public class FoodItems
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Product Name")]
        public string ProductName { get; set; }
    }
}
