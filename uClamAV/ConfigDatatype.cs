using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace uClamAV
{
    public class ConfigDatatype : ConfigurationElement
    {
        [ConfigurationProperty("guid", IsRequired = true)]
        public Guid Guid
        {
            get
            {
                Guid retVal = Guid.Empty;
                if (this["guid"] != null)
                {
                    retVal = new Guid(this["guid"].ToString());
                }

                return retVal;
            }
        }

    } 
}
        