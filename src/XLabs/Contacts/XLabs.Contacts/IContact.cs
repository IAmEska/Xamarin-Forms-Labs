using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Contacts
{
    public interface IContact
    {
        string Id { get; set; }

        bool IsAggregate { get; set; }

        string DisplayName{ get; set; }

        string Prefix{ get; set; }

        string FirstName{ get; set; }

        string MiddleName{ get; set; }

        string LastName{ get; set; }

        string NickName{ get; set; }

        string Suffix{ get; set; }

        IEnumerable<Website> Websites{ get; set; }

        IEnumerable<Organization> Organizations{ get; set; }

        IEnumerable<Note> Notes{ get; set; }

        IEnumerable<Email> Emails{ get; set; }

        IEnumerable<Phone> Phones{ get; set; }

        IEnumerable<Address> Addresses{ get; set; }

        // relationships, instantMessagingAccount

        Task<ImageSource> GetThumbnailAsync();

        Task<ImageSource> GetPhotoAsync();
    }
}

