using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class PhoneNumbersType
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
}
