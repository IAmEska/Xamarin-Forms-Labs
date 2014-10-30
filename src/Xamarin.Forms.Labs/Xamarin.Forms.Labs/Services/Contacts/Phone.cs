using System;

namespace Xamarin.Forms.Labs.Services.Contacts
{
    public class Phone
    {
        public enum PhoneType
        {
            Home,
            HomeFax,
            Work,
            WorkFax,
            Pager,
            Mobile,
            Other
        }

        public Phone()
        {
        }

        public PhoneType Type{ get; set; }

        public string Number{ get; set; }
    }
}

