<h1 align="center">ChaosRadio</h1>
<div align="center">
<a href="https://github.com/MS-crew/ChaosRadio/releases"><img src="https://img.shields.io/github/downloads/MS-crew/ChaosRadio/total?style=for-the-badge&logo=githubactions&label=Downloads" href="https://github.com/MS-crew/ChaosRadio/releases" alt="GitHub Release Download"></a>
<a href="https://github.com/MS-crew/ChaosRadio/releases"><img src="https://img.shields.io/badge/Build-1.1.8-brightgreen?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/ChaosRadio/releases" alt="GitHub Releases"></a>
<a href="https://github.com/MS-crew/ChaosRadio/blob/master/LICENSE"><img src="https://img.shields.io/badge/Licence-GNU_3.0-blue?style=for-the-badge&logo=gitbook" href="https://github.com/MS-crew/ChaosRadio/blob/master/LICENSE" alt="General Public License v3.0"></a>
<a href="https://github.com/ExMod-Team/EXILED"><img src="https://img.shields.io/badge/Exiled-9.3.0-red?style=for-the-badge&logo=gitbook" href="https://github.com/ExMod-Team/EXILED" alt="GitHub Exiled"></a>

</div>

## Chaos Radio

- **Special Channels:** A voice communication network that can only be heard with a chaos radio.
- **Add to adjustable inventory:** Can be added automatically when chaos arises.

## Installation

1. Download the release file from the GitHub page [here](https://github.com/MS-crew/ChaosRadio/releases).
2. Extract the contents into your `\AppData\Roaming\EXILED\Plugins` directory.
3. Configure the plugin according to your server’s needs using the provided settings.
4. Restart your server to apply the changes.

## Feedback and Issues

This is the initial release of the plugin. We welcome any feedback, bug reports, or suggestions for improvements.

- **Report Issues:** [Issues Page](https://github.com/MS-crew/ChaosRadio/issues)
- **Contact:** [discerrahidenetim@gmail.com](mailto:discerrahidenetim@gmail.com)

Thank you for using our plugin and helping us improve it!
## Default Config
```yml
# Whether the plugin is enabled or disabled
is_enabled: true
# debug open or not
debug: false
# Should every chaos insurgency get a chaos radio when they spawn?
add_radioin_spawn: true
# Should chaos insurgency get chaos radio even if it is a Custom Role?
add_even_custom_role: true
chaos_radio:
  id: 311
  weight: 1.70000005
  name: 'Chaos Radio'
  type: Radio
  scale:
    x: 1
    y: 1
    z: 1
  description: 'A special radio for Chaos''s communication network.'
  spawn_properties:
    limit: 2
    dynamic_spawn_points:
    - location: InsideGateA
      chance: 100
    static_spawn_points:
    - name: ""
      chance: 0
      position:
        x: 0
        y: 0
        z: 0
    role_spawn_points: []
    room_spawn_points: []
    locker_spawn_points: []
```
