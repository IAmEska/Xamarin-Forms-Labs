using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace XLabs.Contacts
{
    public interface IAddressBook : IQueryable<IContact>
    {
        Task<bool> RequestPermission();

        IContact Load(string id);

        bool IsReadOnly{ get; }

        bool SingleContactsSupported { get; }

        bool AggregateContactsSupported { get; }

        bool PreferContactAggregation { get; set; }

        bool LoadSupported { get; }

    }
}

