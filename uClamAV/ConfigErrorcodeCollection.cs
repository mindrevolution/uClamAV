using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace uClamAV
{
    class ConfigErrorcodeCollection : ConfigurationElementCollection
    {
        public ConfigErrorcode this[int index]
        {
            get
            {
                return base.BaseGet(index) as ConfigErrorcode;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigErrorcode();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigErrorcode)element).Key;
        }
    }
}