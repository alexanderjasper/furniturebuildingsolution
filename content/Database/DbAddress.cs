using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("Addresses")]
    public class DbAddress : BaseDbEntity
    {
        public string AddressString { get; set; }
        public Guid DawaId { get; set; }
    }
}