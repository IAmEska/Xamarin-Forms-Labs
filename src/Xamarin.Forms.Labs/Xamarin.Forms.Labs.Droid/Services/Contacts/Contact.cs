﻿using System;
using Xamarin.Forms.Labs.Services.Contacts;
using Android.Content;
using Android.Provider;
using Android.Database;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Dependency(typeof(IContact))]
namespace Xamarin.Forms.Labs.Droid.Services.Contacts
{
    public class Contact : IContact
    {
        #region platform specific

        internal List<GroupMembership> groupMemberships;

        public IEnumerable<GroupMembership> GroupMemberships
        {
            get
            {
                return groupMemberships;
            }
            set
            {
                groupMemberships = new List<GroupMembership>(value);
            }
        }

        internal List<InstantMessagingAccount> instantMessagingAccounts;

        public IEnumerable<InstantMessagingAccount> InstantMessagingAccounts
        {
            get
            {
                return instantMessagingAccounts;
            }
            set
            {
                instantMessagingAccounts = new List<InstantMessagingAccount>(value);
            }
        }

        internal List<Relationship> relationships;

        public IEnumerable<Relationship> Relationships
        {
            get
            {
                return relationships;
            }
            set
            {
                relationships = new List<Relationship>(value);
            }
        }

        #endregion

        #region implemented interface properties

        public string Id
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string Prefix{ get; set; }

        public string FirstName{ get; set; }

        public string MiddleName{ get; set; }

        public string LastName{ get; set; }

        public string NickName{ get; set; }

        public string Suffix{ get; set; }

        internal List<Website> websites;

        public IEnumerable<Website> Websites
        {
            get
            {
                return websites;
            }
            set
            {
                websites = new List<Website>(value);
            }
        }

        internal List<Organization> organizations;

        public IEnumerable<Organization> Organizations
        {
            get
            {
                return Organizations;
            }
            set
            {
                organizations = new List<Organization>(value);
            }
        }

        internal List<Note> notes;

        public IEnumerable<Note> Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = new List<Note>(value);
            }
        }

        internal List<Xamarin.Forms.Labs.Services.Contacts.Email> emails;

        public IEnumerable<Xamarin.Forms.Labs.Services.Contacts.Email> Emails
        {
            get
            {
                return emails;
            }
            set
            {
                emails = new List<Xamarin.Forms.Labs.Services.Contacts.Email>(value);
            }
        }

        internal List<Phone> phones;

        public IEnumerable<Phone> Phones
        {
            get
            {
                return phones;
            }
            set
            {
                phones = new List<Phone>(value);
            }
        }

        internal List<Address> addresses;

        public IEnumerable<Address> Addresses
        {
            get
            {
                return addresses;
            }
            set
            {
                addresses = new List<Address>(value);
            }
        }

        #endregion

        #region implemented interface methods

        public async System.Threading.Tasks.Task<ImageSource> GetThumbnailAsync()
        {
            return await Task<ImageSource>.Run(() =>
            {
                byte[] data = GetThumbnailBytes();
                if (data == null)
                {
                    return null;
                }
                else
                {
                    ImageSource image;
                    image = ImageSource.FromStream(() =>
                    {
                        MemoryStream ms = new MemoryStream(data);
                        ms.Seek(0L, SeekOrigin.Begin);
                        return ms;
                    });
                    return image;
            
                }
            });
        }

        public async System.Threading.Tasks.Task<ImageSource> GetPhotoAsync()
        {
            return await Task<ImageSource>.Run(() =>
            {
                long idL;
                if (long.TryParse(Id, out idL))
                {
                    Android.Net.Uri cUri = ContentUris.WithAppendedId(ContactsContract.Contacts.ContentUri, idL);
                    Android.Net.Uri dpUri = Android.Net.Uri.WithAppendedPath(cUri, ContactsContract.Contacts.Photo.DisplayPhoto);
                    try
                    {
                        ImageSource image;
                        image = ImageSource.FromStream(() =>
                        {
                            return content.OpenInputStream(dpUri);
                        });
                        return image;

                    }
                    catch (IOException e)
                    {
                        return null;
                    }
                }
                else
                    return null;
            });
        }

        #endregion

        public Contact()
        {
            CompleteName = new CompleteName();
        }

        byte[] GetThumbnailBytes()
        {
            string lookupColumn = (IsAggregate)
                ? ContactsContract.ContactsColumns.LookupKey
                : ContactsContract.RawContactsColumns.ContactId;

            ICursor c = null;
            try
            {
                c = this.content.Query(ContactsContract.Data.ContentUri, new[] { ContactsContract.CommonDataKinds.Photo.PhotoColumnId, ContactsContract.DataColumns.Mimetype },
                    lookupColumn + "=? AND " + ContactsContract.DataColumns.Mimetype + "=?", new[] { Id, ContactsContract.CommonDataKinds.Photo.ContentItemType }, null);

                while (c.MoveToNext())
                {
                    byte[] tdata = c.GetBlob(c.GetColumnIndex(ContactsContract.CommonDataKinds.Photo.PhotoColumnId));
                    if (tdata != null)
                        return tdata;
                }
            }
            finally
            {
                if (c != null)
                    c.Close();
            }

            return null;
        }

        public bool IsAggregate
        {
            get;
            private set;
        }

        internal Contact(string id, bool isAggregate, ContentResolver content)
        {
            this.content = content;
            IsAggregate = isAggregate;
            Id = id;
        }

        private readonly ContentResolver content;
    }
}

