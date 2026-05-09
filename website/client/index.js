import * as libmmm from "./libmmm.js";

class Mod {
    constructor(associatedMod, description, imageUrl = "") {
        /** @type { libmmm.MonkeMod } */
        this.modObject = associatedMod;

        /** @type { string } */
        this.description = description;

        /** @type { string } */
        this.imageUrl = imageUrl;
    }
}

/** @param { Mod } item */
function toHtmlEntry(item) {
    const fmt = `
    <div class="mod">
        ${item.imageUrl != "" ? `<img class="mod-image" src="${item.imageUrl}">` : ""}
        
        <div class="mod-metadata">
            <p class="mod-title">${item.modObject.name}</p>
            <p class="mod-description">${item.description}</p>
        </div>

        <div class="mod-actions">
            <a href="mmm://install?mods=${item.modObject.toUrlParameter()}" class="mod-button">
                <span class="material-symbols-outlined">install_desktop</span>
                Install
            </a>
        </div>
    </div>
    `;

    return fmt;
}

/** @param { Mod } mod */
function insertMod(mod) {
    const modList = document.getElementById("modlist");
    modList.innerHTML += toHtmlEntry(mod);
}

if (localStorage.getItem("discord_auth") != null)
{
    document.getElementById("login-btn").innerHTML = `${localStorage.getItem("cached_username")}`;
    document.getElementById("login-btn").href = "https://mmm.sirkingbinx.dev/logout";
}