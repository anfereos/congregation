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
        private readonly Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper, IApiService apiService, IBlobHelper blobHelper)
        {
            _apiService = apiService;
            _blobHelper = blobHelper;
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckProfessionAsync();
            await CheckRolesAsync();
            await CheckUsersAsync();
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Member.ToString());
            await _userHelper.CheckRoleAsync(UserType.Teacher.ToString());
        }

        private async Task CheckProfessionAsync()
        {

            if (!_context.Professions.Any())
            {
                _context.Professions.Add(new Profession
                {
                    Name = "Teacher",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Carpenter",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Analyst",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Driver",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Barber",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Administrator",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Chef",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Mechanical",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Electrical technician",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Systems Engineer",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Electromechanical engineer",
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Seller",
                });
                await _context.SaveChangesAsync();
            }
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
                        Name = "District_1.1",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church_1" },
                            new Church { Name = "Church_2" }
                        }
                    },
                    new District
                    {
                        Name = "District_1.2",
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
                        Name = "District_2.1",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church_6" },
                            new Church { Name = "Church_7" }
                        }
                    },
                    new District
                    {
                        Name = "District_2.2",
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

        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                await CheckAdminsAsync();
                await CheckMembersAsync();
                await CheckTeachersAsync();

            }
        }

        private async Task CheckMembersAsync()
        {
            for (int i = 1; i <= 100; i++)
            {
                await CheckUserAsync($"200{i}", $"Member{i}@yopmail.com", UserType.Member);
            }
        }

        private async Task CheckTeachersAsync()
        {
            for (int i = 1; i <= 20; i++)
            {
                await CheckUserAsync($"300{i}", $"Teacher{i}@yopmail.com", UserType.Teacher);
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

            int churchId = _random.Next(1, _context.Churches.Count());
            int professionId = _random.Next(1, _context.Professions.Count());
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
                    Church = await _context.Churches.FindAsync(churchId),
                    Profession = await _context.Professions.FindAsync(professionId),
                    ImageId = imageId
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }
    }
}
