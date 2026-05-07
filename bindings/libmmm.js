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

// Returns true if the mod in the downloadUrl is verified safe.
function modVerified(downloadUrl) {
    return false; // not implemented
}

// Install a mod with the downloadURL with the display name as name.
function installMod(name, downloadUrl) {
    runUri(`mmm://install?mods=${encodeURIComponent(name)}~${encodeURIComponent(downloadUrl)}`);
}

// Install the specified mod loader.
function installLoader(loader) {
    runUri(`mmm://install?loader=${loaderToString(loader)}`);
}

// Opens the game folder.
function openGameFolder(folderPath) {
    runUri(`mmm://open?mode=game`);
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