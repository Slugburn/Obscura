﻿using System;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public class Monolith : IBuildable
    {
        public string Name
        {
            get { return "Monolith"; }
        }

        public void Place(Sector sector)
        {
            throw new NotImplementedException();
        }

    }
}