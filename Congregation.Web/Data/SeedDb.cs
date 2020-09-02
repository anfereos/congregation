using Congregation.Common.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Congregation.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Field_1",
                    Districts = new List<District>
                {
                    new District
                    {
                        Name = "District_1",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church_1" },
                            new Church { Name = "Church_2" },
                            new Church { Name = "Church_3" }
                        }
                    },
                    new District
                    {
                        Name = "District_2",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church_4" },
                            new Church { Name = "Church_5" }
                        }
                    }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Field_2",
                    Districts = new List<District>
                {
                    new District
                    {
                        Name = "District_3",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church_6" },
                            new Church { Name = "Church_7" },
                            new Church { Name = "Church_8" }
                        }
                    },
                    new District
                    {
                        Name = "District_4",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church_9" },
                            new Church { Name = "Church_10" }
                        }
                    }
                }
                });
                await _context.SaveChangesAsync();
            }
        }
    }

}
