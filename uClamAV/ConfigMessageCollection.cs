using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace uClamAV
{
    class ConfigMessageCollection : ConfigurationElementCollection
    {
        public ConfigMessage this[int index]
        {
            get
            {
                return base.BaseGet(index) as ConfigMessage;
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
            return new ConfigMessage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigMessage)element).Key;
        }
    }
}