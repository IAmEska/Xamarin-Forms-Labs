using System;

namespace XLabs.Contacts.Droid
{
    public enum InstantMessagingService
    {
        Aim,
        Msn,
        Yahoo,
        Icq,
        Jabber,
        Other
    }

    public class InstantMessagingAccount
    {
        public InstantMessagingService Service
        {
            get;
            set;
        }

        public string ServiceLabel
        {
            get;
            set;
        }

        public string Account
        {
            get;
            set;
        }
    }
}

