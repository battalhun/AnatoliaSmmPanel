
// Dinamik modal açma fonksiyonu
function openDynamicModal(url) {

    const modalElement = document.getElementById("dynamicModal");

    if (!modalElement) return;

    const modal = new bootstrap.Modal(modalElement);

    document.getElementById("dynamicModalBody").innerHTML =
        `<div class="text-center p-5">
            <div class="spinner-border" role="status"></div>
        </div>`;

    modal.show();

    fetch(url)
        .then(response => response.text())
        .then(res => {
            const container = document.getElementById("dynamicModalBody");
            container.innerHTML = res;

            container.querySelectorAll('script').forEach(oldScript => {
                const newScript = document.createElement('script');
                newScript.textContent = oldScript.textContent;
                document.body.appendChild(newScript);
                document.body.removeChild(newScript);
            });
        })
        .catch(error => {
            document.getElementById("dynamicModalBody").innerHTML = `
                <div class="p-4 text-danger">
                    Bir hata oluştu.
                </div>
            `;
            console.error(error);
        });
}

// Modal boyutunu ayarlama fonksiyonu
function setModalSize(size) {
    // modal-dialog elemanını bul
    const dialog = document.querySelector('#dynamicModal .modal-dialog');

    // Önce tüm olası boyut sınıflarını temizle (çakışmaması için)
    dialog.classList.remove('modal-sm', 'modal-lg', 'modal-xl');

    // Eğer bir boyut belirtilmişse (sm, lg, xl) onu ekle
    if (size) {
        dialog.classList.add('modal-' + size);
    }
}




