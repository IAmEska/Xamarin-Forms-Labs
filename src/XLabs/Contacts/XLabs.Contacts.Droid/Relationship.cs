﻿using System;

namespace XLabs.Contacts.Droid
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

