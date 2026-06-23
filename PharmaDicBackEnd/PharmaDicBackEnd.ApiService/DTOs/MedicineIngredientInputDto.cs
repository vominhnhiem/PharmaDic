using System.ComponentModel.DataAnnotations;

namespace PharmaDicBackEnd.ApiService.DTOs
{
    public class MedicineIngredientInputDto
    {
        [Required(ErrorMessage = "ID Hoạt chất không được để trống.")]
        public int IngredientId { get; set; }
        public string? Amount { get; set; }
    }
}