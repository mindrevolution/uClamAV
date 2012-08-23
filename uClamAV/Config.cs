using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace uClamAV
{
    class Config : ConfigurationSection
    {
        [ConfigurationProperty("active",IsRequired = false)]
        public string Active
        {
            get
            {
                return this["active"] as string;
              
            }
        }
        [ConfigurationProperty("notificationMailFrom", DefaultValue = "", IsRequired = false)]
        public string From_mail
        {
            get
            {
                return this["notificationMailFrom"] as string;
            }
        }
        [ConfigurationProperty("notificationMailTo", DefaultValue = "", IsRequired = false)]
        public string To_mail
        {
            get
            {
                return this["notificationMailTo"] as string;
            }
        }

        [ConfigurationProperty("freshclamUpdateIntervalHours")]
        public string Freshclamintervall
        {
            get
            {

                return this["freshclamUpdateIntervalHours"] as string;
            
            }
        }

        [ConfigurationProperty("clamscanPath", IsRequired = false)]
        public string Clam_path
        {
            get
            {
                return this["clamscanPath"] as string;
            }
        }
          [ConfigurationProperty("freshclamPath", IsRequired = false)]
        public string Freshclam_path
        {
            get
            {
                return this["freshclamPath"] as string;
            }
        }
        [ConfigurationProperty("datatypes")]
        public ConfigDatatypeCollection Datatypes
        {
            get
            {
                return this["datatypes"] as ConfigDatatypeCollection;
            }
        }

        [ConfigurationProperty("messages")]
        public ConfigMessageCollection Message
        {
            get
            {
                return this["messages"] as ConfigMessageCollection;
            }
        }
        [ConfigurationProperty("errorcodes")]
        public ConfigErrorcodeCollection Errorcode
        {
            get

            {
                return this["errorcodes"] as ConfigErrorcodeCollection;
            }
        }
        
        public static Config GetConfig()
        {
            return System.Configuration.ConfigurationManager.GetSection("uClamAV") as Config;
        }
              
        
    }
}
