async function logoutApi() {
    const response = await fetch('https://localhost:7139/api/admin/logout', {
        method: 'POST',
        credentials: 'include', // ensure browser cookies are sent and received
        headers: {
            'Content-Type': 'application/json'
        }
    });
    return response.json();
}
