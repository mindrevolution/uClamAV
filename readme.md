# mindrevolution uClamAV
**Free Anti-Virus for the Umbraco backend**

### Project
* Project Lead: [@samibel](https://twitter.com/#!/samibel)
* Licence: [GNU General Public License (GPL)](http://www.gnu.org/licenses/gpl-3.0)

### Requirements
* Umbraco Version 4.7.2+

### Installation

###  Configuration and Settings

All settings are configured in /config/uclamav.config. We do recommend to install [Config Tree by Tim Geyssens](http://our.umbraco.org/projects/developer-tools/config-tree) to edit this file.

Path values:

    clamscanPath
    freshclamPath
    
Admins can get a notification if an virus was found:

    notificationMailFrom
    notificationMailTo
    
By default only the default upload datatype of Umbraco is scanned for viruses. You can add additional datatypes by their GUID, if neccessary:

    <datatypes>
        <!-- umbraco default "Upload field" control -->
        <add guid="5032a6e6-69e3-491d-bb28-cd31cd11086c"/>
    </datatypes>

The messages shown to the users in the backend can be customized (no lacalization for now).

    <messages>
        <add key="notactive" text="uClamAV deactivated. Edit uClamAV.config"/>
        <add key="virusfound" text="Infected file detected. uClamAV has removed the file. Please contact your system administrator."/>
    </messages>

### Complete initial configuration
## For reference or in case of emergency ;)
    <?xml version="1.0" encoding="utf-8" ?>
    <uClamAV
        active="true"
        clamscanPath="/App_Data/uClamAV/clamav/clamscan.exe"    
        freshclamPath="/App_Data/uClamAV/clamav/freshclam.exe"
        freshclamUpdateIntervalHours="6"
        notificationMailFrom="email@example.com"
        notificationMailTo="email@example.com">
        
        <!-- What datatypes (referenced by guid) to scan for file uploads -->
        <datatypes>
            <!-- umbraco default "Upload field" control -->
            <add guid="5032a6e6-69e3-491d-bb28-cd31cd11086c"/>
        </datatypes>
        
        <!-- Messages shown to CMS users in the backend -->
        <!-- (no dictionary/localization support for now) -->
        <messages>
            <add key="notactive" text="uClamAV deactivated. Edit uClamAV.config"/>
            <add key="virusfound" text="Infected file detected. uClamAV has removed the file. Please contact your system administrator."/>
        </messages>
    
        <!-- ClamAV error levels translated to meaningful text -->
        <!-- Also shown to CMS users in the backend -->
        <errorcodes>
            <add key="-1" text="Cannot find 'clamscan.exe'. Please update configuration file 'uClamAV.config'.) " />
            <add key="0" text="No virus found." />
            <add key="1" text="Virus(es) found." />
            <add key="40" text="Unknown option passed." />
            <add key="50" text="Unknown option passed." />
            <add key="52" text="Not supported file type." />
            <add key="53" text="Can't open directory." />
            <add key="54" text="Can't open file." />
            <add key="55" text="Error reading file." />
            <add key="56" text="Can't stat input file or directory." />
            <add key="57" text="Can't get absolute path name of current working directory." />
            <add key="58" text="I/O error, please check your file system." />
            <add key="59" text="Can't get information about current user from /etc/passwd." />
            <add key="60" text="Can't get information  about  user  'clamav'  (default  name)  from /etc/passwd." />
            <add key="61" text="Can't fork. Too bad." />
            <add key="62" text="Can't initialize logger." />
            <add key="63" text="Can't create temporary files or directories (check permissions)." />
            <add key="64" text="Can't write to temporary directory (please specify another one)." />
            <add key="70" text="Cant allocate and clear memory." />
            <add key="71" text="Can't allocate memory." />
        </errorcodes>
    </uClamAV>

### Signature Updates
Signature updates are handled automatically by the uClamAV plug-in. The update interval can be adjusted by setting

    freshclamUpdateIntervalHours
    
to the desired value. The default is 6 hours. Freshclam is run at this interval to keep the signature files current.