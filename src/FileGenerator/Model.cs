using System;
using System.Collections.Generic;
using System.Text;

namespace FileGenerator
{
    public class Model
    {
       public Dictionary<string, bool> Features = new Dictionary<string, bool>();

        public Model(Dictionary<string, bool> features)
        {
            Features = features;
        }
    }
}
