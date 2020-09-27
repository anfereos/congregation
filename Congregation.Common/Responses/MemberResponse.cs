using Congregation.Common.Enums;
using System;

namespace Congregation.Common.Responses
{
    public class MemberResponse
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Guid ImageId { get; set; }

        public UserType UserType { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://congregationanfereos.azurewebsites.net/images/noimage.png"
        : $"https://Congregationaz.blob.core.windows.net/users/{ImageId}";
    }
}
