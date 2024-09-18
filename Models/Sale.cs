using System;
using System.Collections.Generic;

namespace ChartAPI.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public int? Price { get; set; }
}
