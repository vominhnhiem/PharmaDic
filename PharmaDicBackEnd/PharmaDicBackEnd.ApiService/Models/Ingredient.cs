using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<MedicineIngredient> MedicineIngredients { get; set; } = new List<MedicineIngredient>();
}
