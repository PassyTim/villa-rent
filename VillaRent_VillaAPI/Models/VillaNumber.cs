using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaRent_VillaAPI.Models;

public class VillaNumber
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VillaNo { get; set; }
    public string? Details { get; set; }
    public int VillaId { get; set; }
    public Villa Villa { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}