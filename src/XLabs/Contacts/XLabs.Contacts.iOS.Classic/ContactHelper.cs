﻿using System;
using System.Linq;
using MonoTouch.AddressBook;
using MonoTouch.Foundation;
using System.Globalization;

namespace XLabs.Contacts.iOS
{
    internal class ContactHelper
    {
        internal static Contact GetContact(ABPerson person)
        {
            Contact contact = new Contact(person)
            {
                DisplayName = person.ToString(),
                Prefix = person.Prefix,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                Suffix = person.Suffix,
                NickName = person.Nickname
            };

            contact.Notes = (person.Note != null) ? new [] { new Note { Contents = person.Note } } : new Note[0];

            contact.Emails = person.GetEmails().Select(e => new Email
                {
                    Address = e.Value,
                    Type = GetEmailType(e.Label),
                    Label = (e.Label != null) ? GetLabel(e.Label) : GetLabel(ABLabel.Other)
                });

            contact.Phones = person.GetPhones().Select(p => new Phone
                {
                    Number = p.Value,
                    Type = GetPhoneType(p.Label),
                    Label = (p.Label != null) ? GetLabel(p.Label) : GetLabel(ABLabel.Other)
                });

            Organization[] orgs;
            if (person.Organization != null)
            {
                orgs = new Organization[1];
                orgs[0] = new Organization
                {
                    Name = person.Organization,
                    ContactTitle = person.JobTitle,
                    Type = OrganizationType.Work,
                    Label = GetLabel(ABLabel.Work)
                };
            }
            else
                orgs = new Organization[0];

            contact.Organizations = orgs;

            contact.InstantMessagingAccounts = person.GetInstantMessages().Select(ima => new InstantMessagingAccount()
                {
                    Service = GetImService((NSString)ima.Value[ABPersonInstantMessageKey.Service]),
                    ServiceLabel = (NSString)ima.Value[ABPersonInstantMessageKey.Service],
                    Account = (NSString)ima.Value[ABPersonInstantMessageKey.Username]
                });

            contact.Addresses = person.GetAddresses().Select(a => new Address()
                {
                    Type = GetAddressType(a.Label),
                    Label = (a.Label != null) ? GetLabel(a.Label) : GetLabel(ABLabel.Other),
                    StreetAddress = (NSString)a.Value[ABPersonAddressKey.Street],
                    City = (NSString)a.Value[ABPersonAddressKey.City],
                    Region = (NSString)a.Value[ABPersonAddressKey.State],
                    Country = (NSString)a.Value[ABPersonAddressKey.Country],
                    PostalCode = (NSString)a.Value[ABPersonAddressKey.Zip]
                });

            contact.Websites = person.GetUrls().Select(url => new Website
                {
                    Address = url.Value
                });

            contact.Relationships = person.GetRelatedNames().Select(p => new Relationship
                {
                    Name = p.Value,
                    Type = GetRelationType(p.Label)
                });

            return contact;
        }

        internal static string GetLabel(NSString label)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ABAddressBook.LocalizedLabel(label));
        }

        internal static RelationshipType GetRelationType(string label)
        {
            if (label == ABPersonRelatedNamesLabel.Spouse || label == ABPersonRelatedNamesLabel.Friend)
                return RelationshipType.SignificantOther;
            if (label == ABPersonRelatedNamesLabel.Child)
                return RelationshipType.Child;

            return RelationshipType.Other;
        }

        internal static EmailType GetEmailType(string label)
        {
            if (label == ABLabel.Home)
                return EmailType.Home;
            if (label == ABLabel.Work)
                return EmailType.Work;

            return EmailType.Other;
        }

        internal static PhoneType GetPhoneType(string label)
        {
            if (label == ABLabel.Home)
                return PhoneType.Home;
            if (label == ABLabel.Work)
                return PhoneType.Work;
            if (label == ABPersonPhoneLabel.Mobile || label == ABPersonPhoneLabel.iPhone)
                return PhoneType.Mobile;
            if (label == ABPersonPhoneLabel.Pager)
                return PhoneType.Pager;
            if (label == ABPersonPhoneLabel.HomeFax)
                return PhoneType.HomeFax;
            if (label == ABPersonPhoneLabel.WorkFax)
                return PhoneType.WorkFax;

            return PhoneType.Other;
        }

        internal static InstantMessagingService GetImService(string service)
        {
            if (service == ABPersonInstantMessageService.Aim)
                return InstantMessagingService.Aim;
            if (service == ABPersonInstantMessageService.Icq)
                return InstantMessagingService.Icq;
            if (service == ABPersonInstantMessageService.Jabber)
                return InstantMessagingService.Jabber;
            if (service == ABPersonInstantMessageService.Msn)
                return InstantMessagingService.Msn;
            if (service == ABPersonInstantMessageService.Yahoo)
                return InstantMessagingService.Yahoo;

            return InstantMessagingService.Other;
        }

        internal static AddressType GetAddressType(string label)
        {
            if (label == ABLabel.Home)
                return AddressType.Home;
            if (label == ABLabel.Work)
                return AddressType.Work;

            return AddressType.Other;
        }
    }
}

