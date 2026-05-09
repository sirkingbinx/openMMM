package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"net/http"
	"net/url"
	"os"

	_ "github.com/go-sql-driver/mysql"
	disgoauth "github.com/realTristan/disgoauth"
)

func main() {
	root_url, err := os.ReadFile("web-url.key")
	if err != nil {
		fmt.Println(err)
	}

	discord_oauth2_clientid, err := os.ReadFile("discord-oauth2-clientid.key")
	if err != nil {
		fmt.Println(err)
	}

	discord_oauth2_clientkey, err := os.ReadFile("discord-oauth2-clientsecret.key")
	if err != nil {
		fmt.Println(err)
	}

	// discord auth handler
	var dc *disgoauth.Client = disgoauth.Init(&disgoauth.Client{
		ClientID:     string(discord_oauth2_clientid),
		ClientSecret: string(discord_oauth2_clientkey),
		RedirectURI:  fmt.Sprintf("%s/redirect", root_url),
		Scopes:       []string{disgoauth.ScopeIdentify},
	})

	http.HandleFunc("/login", func(w http.ResponseWriter, r *http.Request) {
		fmt.Printf("/login from %s", r.RemoteAddr)
		dc.RedirectHandler(w, r, "")
	})

	http.HandleFunc("/logout", func(w http.ResponseWriter, r *http.Request) {
		fmt.Printf("/logout from %s", r.RemoteAddr)

		token := r.URL.Query().Get("token")

		data := map[string]string{"client_id": string(discord_oauth2_clientid), "client_secret": string(discord_oauth2_clientkey), "token": token}
		jsonData, _ := json.Marshal(data)

		resp, err := http.Post("https://discord.com/api/oauth2/token/revoke", "application/x-www-form-urlencoded", bytes.NewBuffer(jsonData))
		if err != nil {
			fmt.Printf("uh oh! client token revoke error %v", err)
		}

		defer resp.Body.Close()
	})

	http.HandleFunc("/redirect", func(w http.ResponseWriter, r *http.Request) {
		var (
			codeFromURLParamaters = r.URL.Query()["code"][0]
			accessToken, _        = dc.GetOnlyAccessToken(codeFromURLParamaters)
			userData, _           = disgoauth.GetUserData(accessToken)
		)

		http.Redirect(w, r, fmt.Sprintf("https://mmm.sirkingbinx.dev/login?token=%s&user=%s", url.QueryEscape(accessToken), url.QueryEscape(userData["username"].(string))), http.StatusAccepted)
	})

	http.ListenAndServe(":8000", nil)
}
