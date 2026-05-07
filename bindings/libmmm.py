# MonkeModManager/bindings/libmmm.py
# Bindings for the mmm protocol in Python

# 
# This is free and unencumbered software released into the public domain.
# 
# Anyone is free to copy, modify, publish, use, compile, sell, or
# distribute this software, either in source code form or as a compiled
# binary, for any purpose, commercial or non-commercial, and by any
# means.
# 
# In jurisdictions that recognize copyright laws, the author or authors
# of this software dedicate any and all copyright interest in the
# software to the public domain. We make this dedication for the benefit
# of the public at large and to the detriment of our heirs and
# successors. We intend this dedication to be an overt act of
# relinquishment in perpetuity of all present and future rights to this
# software under copyright law.
# 
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
# EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
# MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
# IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
# OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
# ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
# OTHER DEALINGS IN THE SOFTWARE.
# 
# For more information, please refer to <http://unlicense.org>
# 

from webbrowser import open
from urllib.parse import quote

from enum import Enum

class ModLoader(Enum):
    BEPINEX = 0,
    MELONLOADER = 1,

class ModFolder(Enum):
    GAME_FOLDER = 0,
    PLUGINS_FOLDER = 1,
    USER_DATA_FOLDER = 2,
    APP_DATA_FOLDER = 3

def install_mod(name: str, url: str):
    """Install a mod with the given display name and URL."""
    open(f"mmm://install?mods={quote(name)}~{quote(url)}")

def install_loader(loader: ModLoader):
    """Install the specified mod loader."""
    open(f"mmm://install?loader={ "BepInEx" if loader is 0 else "MelonLoader" }")

def open_folder(folder: ModFolder):
    """Open a folder in the File Explorer."""
    folderStr = ""

    match folder:
        case 0:
            folderStr = "game"
        case 1:
            folderStr = "mods"
        case 2:
            folderStr = "userdata",
        case 3:
            folderStr = "appdata"
    
    open(f"mmm://install?mode={folderStr}")

def open_path(path: str):
    """Open a folder in the File Explorer. The path should be relative to Gorilla Tag's folder."""
    open(f"mmm://install?path={quote(path)}")