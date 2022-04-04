// Config object to be passed to Msal on creation.
// For a full list of msal.js configuration parameters, 
// visit https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/configuration.md
const msalConfig = {
    auth: {
        clientId: "893b158b-b104-411b-a2eb-f7764c0f3ea7",
        authority: "https://login.microsoftonline.com/66e853de-ece3-44dd-9d66-ee6bdf4159d4", // do not forget to replace with your real tenant domain
        redirectUri: "https://yellow-island-0aa53d803.azurestaticapps.net",
    },
    cache: {
        cacheLocation: "sessionStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    },
    system: {
        loggerOptions: {
            loggerCallback: (level, message, containsPii) => {
                if (containsPii) {	
                    return;	
                }	
                switch (level) {	
                    case msal.LogLevel.Error:	
                        console.error(message);	
                        return;	
                    case msal.LogLevel.Info:	
                        console.info(message);	
                        return;	
                    case msal.LogLevel.Verbose:	
                        console.debug(message);	
                        return;	
                    case msal.LogLevel.Warning:	
                        console.warn(message);	
                        return;	
                }
            }
        }
    }
};

// Add here the scopes that you would like the user to consent during sign-in
const loginRequest = {
    scopes: ["api://4a655c68-c7ec-4f3d-ab1f-463125368f9a/user_impersonation"] //this is the *scope* string for the exposed scope of WebAPI app registration
};

// Add here the scopes to request when obtaining an access token for MS Graph API
const tokenRequest = {
    scopes: ["api://4a655c68-c7ec-4f3d-ab1f-463125368f9a/user_impersonation"], //this is the *scope* string for the exposed scope of WebAPI app registration
    forceRefresh: false // Set this to "true" to skip a cached token and go to the server to get a new token
};
