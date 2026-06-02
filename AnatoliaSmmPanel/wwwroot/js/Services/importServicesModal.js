
// 1. Senkronizasyon Butonu Yükleniyor Efekti
function showSyncLoading(form) {
    const btn = form.querySelector('button');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Synchronizing...';
}

// 2. Tabloda Anlık Arama (Filtreleme) Fonksiyonu
function filterProvidersTable() {
    const input = document.getElementById("providers-search");
    const filter = input.value.toLowerCase();
    const table = document.getElementById("providersTable");
    const rows = table.getElementsByTagName("tr");

    // Başlığı (thead) atlamak için döngüye 1'den başlıyoruz
    for (let i = 1; i < rows.length; i++) {
        // Sağlayıcı adının bulunduğu ilk sütunu hedef alıyoruz
        let nameColumn = rows[i].querySelector(".provider-name");

        if (nameColumn) {
            let nameValue = nameColumn.textContent || nameColumn.innerText;
            // Aranan kelime sağlayıcı adında varsa satırı göster, yoksa gizle
            if (nameValue.toLowerCase().indexOf(filter) > -1) {
                rows[i].style.display = "";
            } else {
                rows[i].style.display = "none";
            }
        }
    }
}


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
            document.getElementById("dynamicModalBody").innerHTML = res;
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




//$(document).on("change", "#provider_id", function () {

//    let providerId = $(this).val();

//    $.ajax({
//        url: "/admin/services/get-provider-services",
//        type: "POST",
//        data: {
//            providerId: providerId,
//            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
//        },
//        success: function (response) {
//            $(".import-list-container").html(response);
//        }
//    });

//});




$(document).ready(function () {

    // Category Select All checkbox
    $(document).on('change', '[id^="select_all_"]', function () {

        const isChecked = $(this).prop('checked');

        // örnek: select_all_5 → 5
        const categoryId = $(this).attr('id').replace('select_all_', '');

        // hedef collapse alanı
        const targetId = '#collapse_' + categoryId;

        // sadece o kategori altındaki service checkbox'ları seç
        $(targetId)
            .find('input[type="checkbox"]')
            .prop('checked', isChecked);
    });

});
