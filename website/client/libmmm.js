// MonkeModManager/bindings/libmmm.js
// Bindings for the MMM protocol in JavaScript

// 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>
// 

/**
 * @description Represents a mod to be managed by MonkeModManager.
 */
export class MonkeMod {
    constructor(name, downloadURL) {
        /**
         * @type {string}
         * @description The display name used on MonkeModManager.
         */
        this.name = name;

        /**
         * @type {string}
         * @description The direct download URL used to fetch the mod's file (either a DLL or a ZIP). Zip files will be extracted to Gorilla Tag's game folder, while DLLs are just added to the mod loader's plugins folder.
         */
        this.downloadURL = downloadURL;

        /**
         * @type {Array} 
         * @description A list of dependencies (MonkeMod). These will automatically be included in install requests made through libmmm.
         */
        this.dependencies = [];
    }

    /**
     * @description Converts the monkemod into parameters used in mmm://install queries.
     */ 
    toUrlParameter() {
        let dependencyString = "";
        this.dependencies.forEach(element => {
            dependencyString += "-" + element.toUrlParameter();
        });

        return `${encodeURIComponent(this.name).replace(/\./g, '%2E')}~${encodeURIComponent(this.downloadURL).replace(/\./g, '%2E')}${dependencyString}`;
    }

    /** 
     * @description Returns true if the mod is verified on the MonkeModManager verification registry.
     */
    static async isVerified() {
        // Holy shit, this is ugly! Thanks ECMAScript!
        let verificationListResult = await fetch("https://raw.githubusercontent.com/sirkingbinx/openMMM/refs/heads/master/api/Verified.txt");
        let lines = (await verificationListResult.text()).split(/\r?\n/).filter(line => line.trim() !== "");

        return lines.some(verifiedUrl => this.downloadURL.startsWith(verifiedUrl));
    }
}

/**
 * @param {MonkeMod} mod - Mod to install. Automatically includes it's dependencies.\
 * @description Install a mod.
 */
export function installMod(mod) {
    runUri(`mmm://install?mods=${mod.toUrlParameter()}`);
}

/**
 * @param {MonkeMod} mod - Mod to install. Automatically includes it's dependencies.
 * @description Install the specified mod loader.
 */
export function installLoader(loader) {
    runUri(`mmm://install?loader=${loaderToString(loader)}`);
}

/** @description Opens the game folder. */
export function openGameFolder() {
    runUri(`mmm://open?mode=game`);
}

/** @description Opens the mods folder. */
export function openModsFolder() {
    runUri(`mmm://open?mode=mods`);
}

/** @description Opens the config/userdata folder. */
export function openUserDataFolder() {
    runUri(`mmm://open?mode=userdata`);
}

/** @description Opens Gorilla Tag's AppData folder. */
export function openAppDataFolder() {
    runUri(`mmm://open?mode=appdata`);
}

/** @description Opens a folder (path relative to Gorilla Tag folder) */
export function openFolder(folderPath) {
    runUri(`mmm://open?path=${encodeURIComponent(folderPath)}`);
}

/** @description Represents a mod loader. */
export const Loader = Object.freeze({
    /** @description BepInEx */
    BEPINEX: 'BepInEx',

    /** @description MelonLoader */
    MELONLOADER: 'MelonLoader',
});

function runUri(uri) {
    const link = document.createElement("a");
    link.href = uri;

    document.body.appendChild(link);

    link.click();
    link.remove();
}

function loaderToString(loader) {
    return loader === Loader.BEPINEX ? "BepInEx" : "MelonLoader";
}