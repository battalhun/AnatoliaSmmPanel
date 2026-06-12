
// Modal boyutunu ayarlamak ve içeriği yüklemek için kullanılan fonksiyon
function importbutton() {
    setModalSize('xl');
    openDynamicModal('/admin/services/import');
}

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


// 3. Kategori Bazında Tümünü Seç / Tümünün Seçimini Kaldırma
$(document).ready(function () {

    $(document).on('change', '[id^="select_all_"]', function () {
        const isChecked = $(this).prop('checked');
        const categoryId = $(this).attr('id').replace('select_all_', '');
        const targetId = '#collapse_' + categoryId;
        $(targetId).find('input[type="checkbox"]').prop('checked', isChecked);

        updateSelectedCount(); 
    });

});

// 4. Seçilen Hizmet Sayısını Güncelleme ve Devam Butonunu Aktif/Pasif Yapma
$(function () {

    updateSelectedCount();

    $(document).on('change', '.service-checkbox', function () {
        updateSelectedCount();
    });

    $(document).on('click', '#btnContinue', function () {

        const container = $('#selectedServicesContainer');

        container.empty();

        $('.service-checkbox:checked').each(function () {

            const serviceId = $(this).data('service-id');

            container.append(`
                <input type="hidden"
                       name="SelectedServiceIds"
                       value="${serviceId}" />
            `);
        });

        $('#importForm').submit();
    });

    // Seçilen hizmet sayısını güncelleyen yardımcı fonksiyon
    function updateSelectedCount() {

        const count = $('.service-checkbox:checked').length;

        $('#selectedCount').text(count);

        $('#btnContinue').prop('disabled', count === 0);
    }
});