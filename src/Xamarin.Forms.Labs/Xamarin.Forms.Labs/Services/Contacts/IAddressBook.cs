using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Xamarin.Forms.Labs.Services.Contacts
{
    public interface IAddressBook : IQueryable<IContact>
    {
        Task<bool> RequestPermission();

        IEnumerator<IContact> GetEnumerator();

        IContact Load(string id);

    }
}

