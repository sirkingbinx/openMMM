const params = new URLSearchParams(window.location.search);

if (params.get("token") != null) {
    // callback is done
    localStorage.setItem("discord_auth", params.get("token"));
    localStorage.setItem("cached_username", params.get("user"));
    
    window.location.replace("https://mmm.sirkingbinx.dev");
} else {
    window.location.replace("https://api.mmm.sirkingbinx.dev/login")
}