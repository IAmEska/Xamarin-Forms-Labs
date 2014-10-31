using System;

namespace XLabs.Contacts
{
    public enum EmailType
    {
        Home,
        Work,
        Other
    }

    public class Email
    {
        public string Label{ get; set; }

        public EmailType Type{ get; set; }

        public string Address{ get; set; }

    }
}

