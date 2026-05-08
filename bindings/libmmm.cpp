// MonkeModManager/bindings/libmmm.cc
// Bindings for the MMM protocol in C++

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

#include <iostream>
#include <string>
#include <vector>
#include <sstream>
#include <iomanip>
#include <cctype>

std::string urlEncode(const std::string &value) {
    std::ostringstream escaped;
    escaped.fill('0');
    escaped << std::hex;

    for (unsigned char c : value) {
        if (std::isalnum(c) || c == '-' || c == '_' || c == '.' || c == '~') {
            escaped << c;
        } else {
            escaped << std::uppercase << '%' << std::setw(2) << int(c);
        }
    }

    return escaped.str();
}

struct MonkeMod {
    std::string name;
    std::string url;
    std::vector<MonkeMod> dependencies;
};

void libmmm_runURI(std::string uri) {
    std::string cmd = "start " + uri;
    std::system(cmd.c_str());
}

std::string libmmm_modToUrlQuery(MonkeMod m)
{
    std::string urlName = urlEncode(m.name);
    std::string urlLink = urlEncode(m.url);
    
    std::string dependencyStr = std::string();

    for (auto &&i : m.dependencies)
    {
        MonkeMod dependency = m;
        dependencyStr += "-" + libmmm_modToUrlQuery(dependency);
    }
    
    return (urlName + "~" + urlLink) + dependencyStr;
}

void libmmm_install(MonkeMod m)
{
    libmmm_runURI("mmm://install?mods=" + libmmm_modToUrlQuery(m));
}

void libmmm_install(std::string name, std::string url)
{
    libmmm_runURI("mmm://install?mods=" + urlEncode(name) + "~" + urlEncode(url));
}