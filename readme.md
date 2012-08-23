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

### Signature Updates
Signature updates are handled automatically by the uClamAV plug-in. The update interval can be adjusted by setting

    freshclamUpdateIntervalHours
    
to the desired value. The default is 6 hours. Freshclam is run at this interval to keep the signature files current.