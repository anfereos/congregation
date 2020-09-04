using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Congregation.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboProfessions();
    }

}
