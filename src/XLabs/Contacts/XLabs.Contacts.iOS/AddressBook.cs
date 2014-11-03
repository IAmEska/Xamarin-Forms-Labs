using System;
using System.Linq;
using System.Linq.Expressions;
using AddressBook;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(XLabs.Contacts.iOS.AddressBook))]
namespace XLabs.Contacts.iOS
{
    public class AddressBook : IAddressBook
    {
        public AddressBook()
        {
        }

        #region IAddressBook implementation

        public System.Threading.Tasks.Task<bool> RequestPermission()
        {
            var tcs = new TaskCompletionSource<bool>();
            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
            {
                var status = ABAddressBook.GetAuthorizationStatus();
                if (status == ABAuthorizationStatus.Denied || status == ABAuthorizationStatus.Restricted)
                    tcs.SetResult(false);
                else
                {
                    if (this.addressBook == null)
                    {
                        this.addressBook = new ABAddressBook();
                        this.provider = new ContactQueryProvider(this.addressBook);
                    }

                    if (status == ABAuthorizationStatus.NotDetermined)
                    {
                        this.addressBook.RequestAccess((s, e) =>
                            {
                                tcs.SetResult(s);
                                if (!s)
                                {
                                    this.addressBook.Dispose();
                                    this.addressBook = null;
                                    this.provider = null;
                                }
                            });
                    }
                    else
                        tcs.SetResult(true);
                }
            }
            else
                tcs.SetResult(true);

            return tcs.Task;
        }

        public IContact Load(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id");

            CheckStatus();

            int rowId;
            if (!Int32.TryParse(id, out rowId))
                throw new ArgumentException("Not a valid contact ID", "id");

            ABPerson person = this.addressBook.GetPerson(rowId);
            if (person == null)
                return null;

            return ContactHelper.GetContact(person);
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public bool SingleContactsSupported
        {
            get
            {
                return true;
            }
        }

        public bool AggregateContactsSupported
        {
            get
            {
                return true;
            }
        }

        public bool PreferContactAggregation
        {
            get;
            set;
        }

        public bool LoadSupported
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region IEnumerable implementation

        public System.Collections.Generic.IEnumerator<IContact> GetEnumerator()
        {
            CheckStatus();
            return this.addressBook.GetPeople().Select(ContactHelper.GetContact).GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
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
                return this.provider;
            }
        }

        #endregion

        private ABAddressBook addressBook;
        private IQueryProvider provider;

        private void CheckStatus()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
            {
                var status = ABAddressBook.GetAuthorizationStatus();
                if (status != ABAuthorizationStatus.Authorized)
                    throw new System.Security.SecurityException("AddressBook has not been granted permission");
            }

            if (this.addressBook == null)
            {
                this.addressBook = new ABAddressBook();
                this.provider = new ContactQueryProvider(this.addressBook);
            }
        }

    }
}

