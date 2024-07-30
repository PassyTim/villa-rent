using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaRent.Web.Models.DTO;

namespace VillaRent.Web.Models.ViewModels;

public class VillaNumberCreateViewModel
{
    public VillaNumberCreateViewModel()
    {
        VillaNumber = new VillaNumberCreateDto( 0,0,null);
    }
    public VillaNumberCreateDto VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem>? Villas { get; set; }
}