using System;
using System.Collections.Generic;

namespace BBT.Resource;

[Serializable]
public class ObjectDictionary : Dictionary<string, object?>
{
    public ObjectDictionary()
    {
    }

    public ObjectDictionary(IDictionary<string, object?> dictionary)
        : base(dictionary)
    {
    }
}
