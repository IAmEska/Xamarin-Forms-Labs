using System;

namespace XLabs.Contacts
{
    public enum OrganizationType
    {
        Work,
        Other
    }

    public class Organization
    {
        public string Name{ get; set; }

        public string Label { get; set; }

        public string ContactTitle { get; set; }

        public OrganizationType Type { get; set; }
        //TODO avaible for WP, DROID, IOS
        /*
        public string OfficeLocation{ get; set; }

        public string JobDescription{ get; set; }
        */
    }
}

