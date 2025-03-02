// wwwroot/js/logoutHelper.js
window.logoutApi = async function () {
    const response = await fetch('https://localhost:7139/api/admin/logout', {
        method: 'POST',
        credentials: 'include', // ensure cookies are sent/received
        headers: {
            'Content-Type': 'application/json'
        }
    });
    return response.json();
};