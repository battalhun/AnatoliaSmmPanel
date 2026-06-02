let servicesStore = [];
let groupedServices = {};
let categoryOrder = [];
let collapsedCategories = new Set();

$(document).ready(function () {
    loadReduxServices();

    $('#searchInput, #searchField').on('input change', function () {
        performSearch();
    });
});

function performSearch() {
    const keyword = $('#searchInput').val().toLowerCase().trim();
    const field = $('#searchField').val();

    if (keyword === "") {
        processAndGroupData(servicesStore);
        return;
    }

    const filtered = servicesStore.filter(x => {
        const id = String(x.id).toLowerCase();
        const name = (x.name || "").toLowerCase();
        const provider = (x.provider?.name || "manual").toLowerCase();
        const type = (x.type || "").toLowerCase();

        // Seçilen alana göre filtrele
        switch (field) {
            case 'id':
                return id.includes(keyword);
            case 'name':
                return name.includes(keyword);
            case 'provider':
                return provider.includes(keyword);
            case 'type':
                return type.includes(keyword);
            case 'all':
            default:
                return id.includes(keyword) ||
                    name.includes(keyword) ||
                    provider.includes(keyword) ||
                    type.includes(keyword);
        }
    });

    processAndGroupData(filtered);
}

function loadReduxServices() {
    $.ajax({
        url: '/admin/services/GetReduxAll',
        type: 'GET',
        success: function (response) {
            const responseData = response.$values || response.data || response;
            servicesStore = Array.isArray(responseData) ? responseData : [];
            processAndGroupData(servicesStore);
        }
    });
}

function processAndGroupData(data) {
    groupedServices = {};
    categoryOrder = [];
    $("#serviceTable tbody").empty();

    // 1. Veriyi grupla
    data.forEach(item => {
        const catId = item.serviceCategoryid || 0;
        if (!groupedServices[catId]) {
            groupedServices[catId] = {
                name: item.serviceCategory?.name || item.serviceCategory?.Name || "Uncategorized",
                services: []
            };
            categoryOrder.push(catId);
        }
        groupedServices[catId].services.push(item);
    });

    // 2. Tüm kategorileri tek seferde bas
    categoryOrder.forEach(catId => {
        renderCategory(catId);
    });
}

function renderCategory(catId) {
    const categoryData = groupedServices[catId];
    if (!categoryData) return;

    const isCollapsed = collapsedCategories.has(catId);
    let htmlOutput = '';

    // KATEGORİ BAŞLIĞI
    htmlOutput += `
        <tr class="category-header-row" data-catid="${catId}">
            <td colspan="10" class="p-0 border-0">
                <div class="service-table__category bg-body-tertiary my-2 p-2 border rounded shadow-sm d-flex justify-content-between align-items-center">
                    <div class="d-flex align-items-center gap-2 fw-bold">
                        <span class="fas fa-folder text-warning"></span>
                        <span>${categoryData.name} (${categoryData.services.length})</span>
                    </div>
                    <button class="btn btn-sm ${isCollapsed ? 'btn-primary' : 'btn-outline-secondary'} toggle-services-btn" 
                            data-catid="${catId}">
                        ${isCollapsed ? '<i class="fas fa-eye me-1"></i> Show' : '<i class="fas fa-eye-slash me-1"></i> Hide'} Services
                    </button>   
                </div>
            </td>
        </tr>`;

    // SERVİS SATIRLARI (Collapsed değilse ekle)
    if (!isCollapsed) {
        htmlOutput += generateServiceRowsHtml(catId, categoryData.services);
    }

    $("#serviceTable tbody").append(htmlOutput);
}

function generateServiceRowsHtml(catId, services) {
    let rowsHtml = '';
    services.forEach(item => {
        const providerName = item.provider ? item.provider.name : 'Manual';
        const statusBadge = item.isActive ?
            '<span class="badge bg-success-subtle text-success border border-success-subtle">Enabled</span>' :
            '<span class="badge bg-danger-subtle text-danger border border-danger-subtle">Disabled</span>';

        rowsHtml += `
            <tr class="service-row" data-catid="${catId}">
                <td class="text-center"><input class="form-check-input service-checkbox" type="checkbox" value="${item.id}"></td>
                <td class="text-muted">#${item.id}</td>
                <td class="fw-bold">${item.name}</td>
                <td>${item.type || 'Default'}</td>
                <td>${providerName}</td>
                <td class="text-primary fw-bold">${item.rate}</td>
                <td>${item.min}</td>
                <td>${item.max}</td>
                <td>${statusBadge}</td>
                <td class="text-end">
                     <button class="btn btn-sm btn-light border"><i class="fas fa-edit"></i></button>
                </td>
            </tr>`;
    });
    return rowsHtml;
}

// Buton ile DOM'dan Kaldırma/Ekleme Mantığı (Aynı Kaldı)
$(document).on('click', '.toggle-services-btn', function () {
    const btn = $(this);
    const catId = parseInt(btn.data('catid'));
    const headerRow = btn.closest('tr.category-header-row');
    const categoryData = groupedServices[catId];

    if (collapsedCategories.has(catId)) {
        collapsedCategories.delete(catId);
        headerRow.after(generateServiceRowsHtml(catId, categoryData.services));
        btn.removeClass('btn-primary').addClass('btn-outline-secondary')
            .html('<i class="fas fa-eye-slash me-1"></i> Hide Services');
    } else {
        collapsedCategories.add(catId);
        $(`tr.service-row[data-catid="${catId}"]`).remove();
        btn.removeClass('btn-outline-secondary').addClass('btn-primary')
            .html('<i class="fas fa-eye me-1"></i> Show Services');
    }
});