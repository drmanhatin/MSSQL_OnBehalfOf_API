var msal = require('@azure/msal-node');

const axios = require("axios").default;

const clientConfig = {
      auth: {
            // get this data from the app registration for service principal
            clientId: "fb19b396-910a-45fb-8c7b-b49384b41724",
            clientSecret: "tb-7Q~oEVw4-oFGGzDhhz~2QyW35cv0ntUFfH",

            //tenantid is the tenant id of the service principal
            authority: "https://login.microsoftonline.com/6a791b70-7fd4-48a0-82f0-47ee079e4ccd",
      }
};

const confidentialClientApplication = new msal.ConfidentialClientApplication(clientConfig);

const clientCredentialRequest = {
      scopes: ["api://88ac614f-976d-4473-be3d-49538a1a92a7/.default"]
};


async function getData() {

      //get token
      const result = await confidentialClientApplication.acquireTokenByClientCredential(clientCredentialRequest);
      const accessToken = result.accessToken;

      console.log(accessToken)

      const response = await axios.get("https://apimauthsample.azure-api.net/echo/resource?param1=sample", {

            headers: {"Authorization": `Bearer ${accessToken}`}});

      console.log("response went:", response.statusText);
      return response.data;

}

getData().then(data => console.log(data));

