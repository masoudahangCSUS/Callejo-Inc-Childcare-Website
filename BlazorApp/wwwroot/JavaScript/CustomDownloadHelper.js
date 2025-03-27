console.log("CustomDownloadHelper.js loaded successfully");

window.customDownloadFile = (fileName, byteBase64, contentType) => {
    console.log("customDownloadFile invoked with:", { fileName, contentType });

    try {
        // Convert Base64 string to Blob
        const byteCharacters = atob(byteBase64);
        const byteNumbers = Array.from(byteCharacters, char => char.charCodeAt(0));
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: contentType });

        // Trigger file download
        const link = document.createElement('a');
        const url = URL.createObjectURL(blob);
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    } catch (error) {
        console.error("Error in customDownloadFile:", error);
    }
};