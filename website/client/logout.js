const params = new URLSearchParams(window.location.search);

if (params.get("redir") != null) {
    window.location.replace("https://mmm.sirkingbinx.dev")
} else {
    localStorage.removeItem("cached_username");

    window.location.replace(`https://api.mmm.sirkingbinx.dev/logout?token=${encodeURIComponent(localStorage.getItem("discord_auth"))}`);
    localStorage.removeItem("discord_auth");
}