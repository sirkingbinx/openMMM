package main

import (
	"fmt"
	"net/http"
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
		dc.RedirectHandler(w, r, "")
	})

	http.HandleFunc("/redirect", func(w http.ResponseWriter, r *http.Request) {
		var (
			codeFromURLParamaters = r.URL.Query()["code"][0]
			accessToken, _        = dc.GetOnlyAccessToken(codeFromURLParamaters)
			userData, _           = disgoauth.GetUserData(accessToken)
		)

		fmt.Println(userData)
	})

	http.ListenAndServe(":8000", nil)
}
