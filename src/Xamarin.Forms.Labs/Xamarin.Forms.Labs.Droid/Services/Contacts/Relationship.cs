﻿using System;

namespace Xamarin.Forms.Labs.Droid.Services.Contacts
{
    public enum RelationshipType
    {
        SignificantOther,
        Child,
        Other
    }

    public class Relationship
    {
        public string Name
        {
            get;
            set;
        }

        public RelationshipType Type
        {
            get;
            set;
        }
    }
}

