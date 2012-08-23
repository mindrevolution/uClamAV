using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace uClamAV
{
    public class ConfigMessage : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                string retVal = "";
                if (this["key"] != null)
                {
                    retVal = this["key"] as string;
                }

                return retVal;
            }
        }

        [ConfigurationProperty("text", IsRequired = true)]
        public string Text
        {
            get
            {
                string retVal ="";
                if (this["text"] != null)
                {
                    retVal = this["text"] as string;
                }

                return retVal;
            }
        }

    }
}
