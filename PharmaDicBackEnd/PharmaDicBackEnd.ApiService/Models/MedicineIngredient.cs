using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class MedicineIngredient
{
    public int MedicineId { get; set; }

    public int IngredientId { get; set; }

    public string? Amount { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Medicine Medicine { get; set; } = null!;
}
