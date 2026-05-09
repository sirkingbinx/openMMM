# MonkeModManager
This is a full rewrite of MonkeModManager which achieves multiple things:

- **MonkeModManager is difficult to get mods added to**: You can include an install button on your mod's public page.
- **MonkeModManager's UI is terrible**: Create your own MonkeModManager UI and share it for the world to see.
- **MonkeModManager is old, obsolete, and it's code sucks**: This one is brand new!

There is barely a frontend to this, instead we rely on third-party developers to create their own mod stores.

### Implementation
If you know how to code, there is most likely bindings of the protocol for your language. See [/bindings](/bindings/).

### Verification
Since mod downloads are now community-sourced, mods can be verified to prove that they are not viruses.
Being verified does not show that a mod is "legal" / relies on modded lobbys, it simply shows that a mod is not a virus.

### Download Button
(This example is for BingusNametags++.)

[![Install with MonkeModManager](https://img.shields.io/badge/install_with-MonkeModManager-blue)](https://sirkingbinx.dev/mmm_install?mods=BingusNametags%2B%2B~https%3A%2F%2Fgithub.com%2Fsirkingbinx%2FBingusNametagsPlusPlus%2Freleases%2Flatest%2Fdownload%2FBingusNametagsPlusPlus%2Edll)

```md
<!-- Make sure that your inputs are URL-safe! Encode periods as well (%2E). -->

[![Install with MonkeModManager](https://img.shields.io/badge/install_with-MonkeModManager-blue)](https://sirkingbinx.dev/mmm_install?mods={mod name}~{mod download link)
```

<https://sirkingbinx.dev/mmm_install> will only redirect you to `mmm://install`. This was necessary since GitHub does not allow custom protocol linking.
