export function submitFormWithFile(requestData) {
    return new Promise((resolve, reject) => {
        try {
            // Convert base64 back to binary
            const binaryString = atob(requestData.fileData);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            const blob = new Blob([bytes]);

            // Create FormData
            const formData = new FormData();
            formData.append('File', blob, requestData.fileName);
            formData.append('PdfA', requestData.pdfA);
            formData.append('PdfUA', requestData.pdfUA);

            // Make fetch request with credentials to include cookies
            fetch(requestData.url, {
                method: 'POST',
                body: formData
            })
            .then(response => {
                if (response.ok) {
                    // Handle file download
                    return response.blob().then(blob => {
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url;
                        a.download = 'converted-document.pdf';
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
                        document.body.removeChild(a);
                        resolve(true);
                    });
                } else {
                    console.error('Request failed:', response.status, response.statusText);
                    response.text().then(text => {
                        console.error('Response:', text);
                    });
                    resolve(false);
                }
            })
            .catch(error => {
                console.error('Request error:', error);
                resolve(false);
            });
        } catch (error) {
            console.error('Error preparing request:', error);
            resolve(false);
        }
    });
}
