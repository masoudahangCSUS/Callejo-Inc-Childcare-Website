window.downloadFile = (fileName, byteBase64) => {
    const link = document.createElement('a');
    link.href = "data.application/pdf;base64," + byteBase64;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};