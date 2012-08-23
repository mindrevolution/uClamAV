using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace uClamAV
{
    public class ConfigDatatypeCollection : ConfigurationElementCollection
    {
        public ConfigDatatype this[int index]
        {
            get
            {
                return base.BaseGet(index) as ConfigDatatype;
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
            return new ConfigDatatype();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigDatatype)element).Guid;
        }
    }
}
