using Congregation.Common.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Congregation.Common.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public ChurchResponse Church { get; set; }

        public ProfessionResponse Profession { get; set; }

        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://congregationanfereos.azurewebsites.net/images/noimage.png"
            : $"https://Congregationaz.blob.core.windows.net/users/{ImageId}";

        public UserType UserType { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        public ICollection<MeetingResponse> Mettings { get; set; }

        public ICollection<AssistanceResponse> Assitances { get; set; }
    }
}
