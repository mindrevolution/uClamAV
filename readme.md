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

### Signature Updates
Signature updates are handled automatically by the uClamAV plug-in. The update interval can be adjusted by setting

    freshclamUpdateIntervalHours
    
to the desired value. The default is 6 hours. Freshclam is run at this interval to keep the signature files current.