using Congregation.Common.Enums;
using Congregation.Common.Models;
using Congregation.Common.Services;
using Congregation.Web.Data.Entities;
using Congregation.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Congregation.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IApiService _apiService;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IApiService apiService, IBlobHelper blobHelper)
        {
            _apiService = apiService;
            _blobHelper = blobHelper;
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckProfessionAsync();
            await CheckRolesAsync();
            await CheckUsersAsync();
            //await CheckUserAsync("8511", "Andres", "Restrepo", "anfereos@hotmail.com", "314 999 99 99", "Calle Luna Calle Sol", UserType.Admin);
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Member.ToString());
            await _userHelper.CheckRoleAsync(UserType.Teacher.ToString());
        }

        //private async Task<User> CheckUserAsync(
        //    string document,
        //    string firstName,
        //    string lastName,
        //    string email,
        //    string phone,
        //    string address,
        //    UserType userType)
        //{
        //    User user = await _userHelper.GetUserAsync(email);
        //    if (user == null)
        //    {
        //        user = new User
        //        {
        //            FirstName = firstName,
        //            LastName = lastName,
        //            Email = email,
        //            UserName = email,
        //            PhoneNumber = phone,
        //            Address = address,
        //            Document = document,
        //            Church = _context.Churches.FirstOrDefault(),
        //            Profession = _context.Professions.FirstOrDefault(),
        //            UserType = userType
        //        };

        //        await _userHelper.AddUserAsync(user, "123456");
        //        await _userHelper.AddUserToRoleAsync(user, userType.ToString());

        //        string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
        //        await _userHelper.ConfirmEmailAsync(user, token);

        //    }

        //    return user;
        //}

        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                await CheckAdminsAsync();
                await CheckMembersAsync();
            }
        }

        private async Task CheckMembersAsync()
        {
            for (int i = 1; i <= 100; i++)
            {
                await CheckUserAsync($"200{i}", $"member{i}@yopmail.com", UserType.Member);
            }
        }

        private async Task CheckAdminsAsync()
        {
            await CheckUserAsync("1001", "admin1@yopmail.com", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
            string document,
            string email,
            UserType userType)
        {
            RandomUsers randomUsers;

            do
            {
                randomUsers = await _apiService.GetRandomUser("https://randomuser.me", "api");
            } while (randomUsers == null);

            Guid imageId = Guid.Empty;
            RandomUser randomUser = randomUsers.Results.FirstOrDefault();
            string imageUrl = randomUser.Picture.Large.ToString().Substring(22);
            Stream stream = await _apiService.GetPictureAsync("https://randomuser.me", imageUrl);
            if (stream != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(stream, "users");
            }

            //int cityId = _random.Next(1, _context.Cities.Count());
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = randomUser.Name.First,
                    LastName = randomUser.Name.Last,
                    Email = email,
                    UserName = email,
                    PhoneNumber = randomUser.Cell,
                    Address = $"{randomUser.Location.Street.Number}, {randomUser.Location.Street.Name}",
                    Document = document,
                    UserType = userType,
                    Church = _context.Churches.FirstOrDefault(),
                    Profession = _context.Professions.FirstOrDefault(),
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
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

        private async Task CheckProfessionAsync()
        {
            if (!_context.Professions.Any())
            {
                _context.Professions.Add(new Profession
                {
                    Name = "Medico"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Ingeniero"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Veterinario"
                });
                await _context.SaveChangesAsync();
            }

        }
    }

}
