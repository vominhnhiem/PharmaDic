using System;
using System.Collections.Generic;

namespace PharmaDicBackEnd.ApiService.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int UserId { get; set; }

    public int MedicineId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
