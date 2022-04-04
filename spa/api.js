// Helper function to call MS Graph API endpoint 
// using authorization bearer token scheme
function callApi(endpoint, token, callback) {
    endpoint = document.getElementById('apiendpoint').value;
    const headers = new Headers();
    const bearer = `Bearer ${token}`;
    const iuser = 'false'

    headers.append("Authorization", bearer);
    headers.append("iuser", iuser);

    const options = {
        method: "GET",
        headers: headers,

    };

    console.log('request made to API at: ' + new Date().toString());

    fetch(endpoint, options)
        .then(response => response.json())
        .then(response => callback(response, endpoint))
        .catch(error => console.log(error));
}
