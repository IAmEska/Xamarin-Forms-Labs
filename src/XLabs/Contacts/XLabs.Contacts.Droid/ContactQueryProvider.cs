﻿using System;
using System.Collections;
using Android.Content;
using Android.Content.Res;
using System.Linq;
using XLabs.Droid.ContentProvider;

namespace XLabs.Contacts.Droid
{
    internal class ContactQueryProvider : ContentQueryProvider
    {
        internal ContactQueryProvider(ContentResolver content, Resources resources)
            : base(content, resources, new ContactTableFinder())
        {
        }

        public bool UseRawContacts
        {
            get { return ((ContactTableFinder)TableFinder).UseRawContacts; }
            set { ((ContactTableFinder)TableFinder).UseRawContacts = value; }
        }

        protected override IEnumerable GetObjectReader(ContentQueryTranslator translator)
        {
            if (translator == null || translator.ReturnType == null || translator.ReturnType == typeof(Contact))
                return new ContactReader(UseRawContacts, translator, content, resources);
            else if (translator.ReturnType == typeof(Phone))
                return new GenericQueryReader<Phone>(translator, content, resources, ContactHelper.GetPhone);
            else if (translator.ReturnType == typeof(Email))
                return new GenericQueryReader<Email>(translator, content, resources, ContactHelper.GetEmail);
            else if (translator.ReturnType == typeof(Address))
                return new GenericQueryReader<Address>(translator, content, resources, ContactHelper.GetAddress);
            else if (translator.ReturnType == typeof(Relationship))
                return new GenericQueryReader<Relationship>(translator, content, resources, ContactHelper.GetRelationship);
            //TODO implement platform specific items
            /*else if (translator.ReturnType == typeof(GroupMembership))
                return new GenericQueryReader<GroupMembership>(translator, content, resources, ContactHelper.GetContactGroupMembership);
            else if (translator.ReturnType == typeof(InstantMessagingAccount))
                return new GenericQueryReader<InstantMessagingAccount>(translator, content, resources, ContactHelper.GetImAccount);*/
            else if (translator.ReturnType == typeof(Website))
                return new GenericQueryReader<Website>(translator, content, resources, ContactHelper.GetWebsite);
            else if (translator.ReturnType == typeof(Organization))
                return new GenericQueryReader<Organization>(translator, content, resources, ContactHelper.GetOrganization);
            else if (translator.ReturnType == typeof(Note))
                return new GenericQueryReader<Note>(translator, content, resources, ContactHelper.GetNote);
            else if (translator.ReturnType == typeof(string))
                return new ProjectionReader<string>(content, translator, (cur, col) => cur.GetString(col));
            else if (translator.ReturnType == typeof(int))
                return new ProjectionReader<int>(content, translator, (cur, col) => cur.GetInt(col));

            throw new ArgumentException();
        }
    }
}

