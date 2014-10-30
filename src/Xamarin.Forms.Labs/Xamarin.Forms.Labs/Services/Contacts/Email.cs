﻿using System;

namespace Xamarin.Forms.Labs.Services.Contacts
{
    public class Email
    {
        public enum EmailType
        {
            Personal,
            Work,
            Other
        }

        public Email()
        {
        }

        public EmailType Type{ get; set; }

        public string Address{ get; set; }

    }
}

