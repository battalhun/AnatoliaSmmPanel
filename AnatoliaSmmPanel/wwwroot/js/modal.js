
function setModalSize(size) {
    const dialog = document.querySelector('#dynamicModal .modal-dialog');
    dialog.classList.remove('modal-sm', 'modal-lg', 'modal-xl');
    if (size) dialog.classList.add('modal-' + size);
}

function openDynamicModal(url) {
    const modalElement = document.getElementById("dynamicModal");
    if (!modalElement) return;

    const modal = new bootstrap.Modal(modalElement);
    document.getElementById("dynamicModalBody").innerHTML =
        `<div class="text-center p-5"><div class="spinner-border" role="status"></div></div>`;

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
        .catch(() => {
            document.getElementById("dynamicModalBody").innerHTML =
                `<div class="p-4 text-danger">Bir hata oluştu.</div>`;
        });
}