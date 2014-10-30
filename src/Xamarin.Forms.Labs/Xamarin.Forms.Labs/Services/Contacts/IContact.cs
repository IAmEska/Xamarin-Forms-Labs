using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Services.Contacts
{
    public interface IContact
    {
        string Id { get; set; }

        string DisplayName{ get; set; }

        CompleteName CompleteName{ get; set; }

        IEnumerable<Website> Websites{ get; set; }

        IEnumerable<Organization> Organizations{ get; set; }

        IEnumerable<Note> Notes{ get; set; }

        IEnumerable<Email> Emails{ get; set; }

        IEnumerable<Phone> Phones{ get; set; }

        IEnumerable<Address> Addresses{ get; set; }

        Task<ImageSource> GetThumbnailAsync();

        Task<ImageSource> GetPhotoAsync();
    }
}

