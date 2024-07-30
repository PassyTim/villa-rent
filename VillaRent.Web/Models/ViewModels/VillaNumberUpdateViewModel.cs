using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaRent.Web.Models.DTO;

namespace VillaRent.Web.Models.ViewModels;

public class VillaNumberUpdateViewModel
{
    public VillaNumberUpdateViewModel()
    {
        VillaNumber = new VillaNumberUpdateDto( 0,0,null);
    }
    public VillaNumberUpdateDto VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? Villas { get; set; }
}