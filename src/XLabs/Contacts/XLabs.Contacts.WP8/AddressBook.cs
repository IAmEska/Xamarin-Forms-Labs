using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using XLabs.Contacts;
using Xamarin.Forms;

[assembly: Dependency(typeof(XLabs.Contacts.WP.AddressBook))]
namespace XLabs.Contacts.WP
{
    public class AddressBook : IAddressBook
    {
        public AddressBook()
        {
            this.provider = new ContactQueryProvider();
        }

        public Task<bool> RequestPermission()
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var contacts = new Microsoft.Phone.UserData.Contacts();
                    
                    contacts.Accounts.ToArray(); // Will trigger exception if manifest doesn't specify ID_CAP_CONTACTS
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public IContact Load(string id)
        {
            throw new NotSupportedException();
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool SingleContactsSupported
        {
            get { return false; }
        }

        public bool AggregateContactsSupported
        {
            get { return true; }
        }

        public bool PreferContactAggregation
        {
            get;
            set;
        }

        public bool LoadSupported
        {
            get { return false; }
        }

        public IEnumerator<IContact> GetEnumerator()
        {
            return this.provider.GetContacts().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(Contact); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return System.Linq.Expressions.Expression.Constant(this); }
        }

        public System.Linq.IQueryProvider Provider
        {
            get { return this.provider; }
        }

        private readonly ContactQueryProvider provider;
    }
}
