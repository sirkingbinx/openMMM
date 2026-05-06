// MonkeModManager/web/libmmm.js
// Contains utils for using the MMM protocol in JavaScript
// (C) 2026 SirKingBinx

// Returns true if the mod in the downloadUrl is verified safe.
function modVerified(downloadUrl) {
    return false; // not implemented
}

// Install a mod with the downloadURL with the display name as name.
function installMod(name, downloadUrl) {
    runUri(`mmm://install?name=${encodeURIComponent(name)}&url=${encodeURIComponent(downloadUrl)}`);
}

// Install the specified mod loader.
function installLoader(loader) {
    runUri(`mmm://install?loader=${loaderToString(loader)}`);
}

// Opens the game folder.
function openGameFolder(folderPath) {
    runUri(`mmm://open`);
}

// Opens the mods folder.
function openModsFolder(folderPath) {
    runUri(`mmm://open?mode=mods`);
}

// Opens the config/userdata folder.
function openUserDataFolder(folderPath) {
    runUri(`mmm://open?mode=userdata`);
}

// Opens Gorilla Tag's AppData folder.
function openAppDataFolder(folderPath) {
    runUri(`mmm://open?mode=appdata`);
}

// Opens a folder (path relative to Gorilla Tag folder)
function openFolder(folderPath) {
    runUri(`mmm://open?path=${encodeURIComponent(folderPath)}`);
}

// Run a URI without navigating away from the current page
function runUri(uri) {
    const link = document.createElement("a");
    link.href = uri;

    document.body.appendChild(link);

    link.click();
    link.remove();
}

const Loader = Object.freeze({
    BEPINEX: 'BepInEx',
    MELONLOADER: 'MelonLoader',
});

function loaderToString(loader) {
    return loader === Loader.BEPINEX ? "BepInEx" : "MelonLoader";
}