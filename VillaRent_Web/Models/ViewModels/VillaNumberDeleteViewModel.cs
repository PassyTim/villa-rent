using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaRent_Web.Models.DTO;

namespace VillaRent_Web.Models.ViewModels;

public class VillaNumberDeleteViewModel
{
    public VillaNumberDeleteViewModel()
    {
        VillaNumber = new VillaNumberDto( 0,0,null, null);
    }
    public VillaNumberDto VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? Villas { get; set; }
}