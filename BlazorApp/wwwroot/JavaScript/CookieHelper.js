window.fetchLoginData = async (url, loginData) => {
    try {
        console.log("fetchLoginData called with:", url, loginData);
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include', // ensures cookies are sent/received
            body: JSON.stringify(loginData)
        });

        if (!response.ok) {
            console.error("Fetch error, status:", response.status);
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const resultText = await response.text();
        console.log("fetchLoginData received:", resultText);
        return resultText;
    } catch (e) {
        console.error("Error in fetchLoginData:", e);
        throw e;
    }
};
