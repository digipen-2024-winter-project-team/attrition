﻿using System.Collections.Generic;

namespace Attrition.Names.Generation
{
    public interface INameData
    {
        List<string> Prefixes { get; }
        List<string> Roots { get; }
        List<string> Suffixes { get; }
    }
}
