using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Congregation.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboCountries();

        IEnumerable<SelectListItem> GetComboDistricts(int countryId);

        IEnumerable<SelectListItem> GetComboChurches(int districtId);

        IEnumerable<SelectListItem> GetComboProfessions();

    }

}
