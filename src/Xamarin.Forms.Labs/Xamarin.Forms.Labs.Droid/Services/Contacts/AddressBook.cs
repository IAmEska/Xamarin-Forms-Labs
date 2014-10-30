using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Provider;
using Android.App;

using Xamarin.Forms.Labs.Services.Contacts;
using Xamarin.Forms;

[assembly: Dependency(typeof(IAddressBook))]
namespace Xamarin.Forms.Labs.Droid.Services.Contacts
{
    public class AddressBook : IAddressBook
    {
        public AddressBook()
        {
            content = Application.Context.ContentResolver;
            resources = Application.Context.Resources;
            this.contactsProvider = new ContactQueryProvider(content, resources);
        }

        #region IAddressBook implementation

        public async Task<bool> RequestPermission()
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    ICursor cursor = this.content.Query(ContactsContract.Data.ContentUri, null, null, null, null);
                    cursor.Dispose();

                    return true;
                }
                catch (Java.Lang.SecurityException)
                {
                    return false;
                }
            });
        }



        public IContact Load(string id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<IContact> GetEnumerator()
        {
            return ContactHelper.GetContacts(!PreferContactAggregation, this.content, this.resources).GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IQueryable implementation

        public Type ElementType
        {
            get
            {
                return typeof(Contact);
            }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get
            {
                return Expression.Constant(this);
            }
        }

        public System.Linq.IQueryProvider Provider
        {
            get
            {
                return this.contactsProvider;
            }
        }

        #endregion

        public bool PreferContactAggregation
        {
            get { return !this.contactsProvider.UseRawContacts; }
            set { this.contactsProvider.UseRawContacts = !value; }
        }

        private readonly ContactQueryProvider contactsProvider;
        private readonly ContentResolver content;
        private readonly Resources resources;
    }
}

