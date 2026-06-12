let servicesStore = [];
let filteredServices = [];
let displayedPages = [];
const pageSize = 50;
const maxPagesInDom = 5;

$(document).ready(function () {
    loadReduxServices();

    $(document).on('change', '#checkAll', function () {
        const isChecked = $(this).prop('checked');
        $('.service-checkbox').prop('checked', isChecked);

        if (isChecked) {
            $('.service-row').addClass('table-active');
        } else {
            $('.service-row').removeClass('table-active');
        }
    });

    $(document).on('change', '.service-checkbox', function () {
        const isChecked = $(this).is(':checked');
        $(this).closest('tr').toggleClass('table-active', isChecked);

        const totalInDom = $('.service-checkbox').length;
        const checkedInDom = $('.service-checkbox:checked').length;
        $('#checkAll').prop('checked', totalInDom === checkedInDom);
    });

    $("#scrollArea").on("scroll", function () {
        const container = $(this);
        const scrollTop = container.scrollTop();
        const scrollHeight = container[0].scrollHeight;
        const innerHeight = container.innerHeight();

        if (scrollTop + innerHeight >= scrollHeight - 600) {
            const lastPage = Math.max(...displayedPages);
            if (lastPage * pageSize < filteredServices.length) {
                renderRows(lastPage + 1, 'append');
            }
        }

        if (scrollTop <= 400 && Math.min(...displayedPages) > 1) {
            const prevPage = Math.min(...displayedPages) - 1;
            renderRows(prevPage, 'prepend');
        }
    });

    $('#searchInput').on('input', function () {
        const keyword = $(this).val().toLowerCase().trim();
        filteredServices = servicesStore.filter(x =>
            (x.name && x.name.toLowerCase().includes(keyword)) ||
            String(x.id).includes(keyword)
        );

        displayedPages = [];
        $("#serviceTable tbody").empty();
        renderRows(1, 'append');
        $("#scrollArea").scrollTop(0);
        $('#checkAll').prop('checked', false);
    });
});

function loadReduxServices() {
    $.ajax({
        url: '/admin/services/GetReduxAll',
        type: 'GET',
        success: function (response) {
            const responseData = response.$values || response.data || response;
            if (responseData) {
                servicesStore = Array.isArray(responseData) ? responseData : [];
                filteredServices = [...servicesStore];
                renderRows(1, 'append');
            }
        }
    });
}

function renderRows(page, direction) {
    if (displayedPages.includes(page)) return;

    const start = (page - 1) * pageSize;
    const end = start + pageSize;
    const pageData = filteredServices.slice(start, end);

    if (pageData.length === 0) return;

    const isAllChecked = $('#checkAll').is(':checked');
    let htmlOutput = '';
    let lastCategoryId = null;

    $.each(pageData, function (i, item) {
        // --- KRİTİK DÜZELTME: Backend'deki isimlendirme 'serviceCategoryid' veya 'ServiceCategoryid' olabilir ---
        // item.serviceCategoryid senin Action'da tanımladığın isimdir.
        const currentCatId = item.serviceCategoryid;

        // --- KATEGORİ BAŞLIĞI EKLEME ---
        if (currentCatId !== lastCategoryId) {
            // Backend'den 'ServiceCategory' nesnesi içinde 'Name' geliyor
            const categoryName = item.serviceCategory?.name || item.serviceCategory?.Name || "Uncategorized";

            htmlOutput += `
                <tr class="category-header-row" data-page="${page}">
                    <td colspan="10" class="p-0 border-0">
                        <div class="service-table__category position-relative w-100 bg-body-tertiary my-2" id="category-${currentCatId}">
                            <div class="d-flex align-items-center justify-content-between p-2 border rounded shadow-sm">
                                <div class="d-flex align-items-center gap-3">
                                    <span class="service-table__handle cursor-pointer text-muted">
                                        <i class="fas fa-grip-vertical"></i>
                                    </span>
                                    <div class="d-flex align-items-center gap-2 fw-bold">
                                        <span class="fas fa-folder text-warning"></span>
                                        <span>${categoryName}</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center gap-3">
                                    <div class="dropdown">
                                        <button class="btn btn-sm btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                                            Actions
                                        </button>
                                        <ul class="dropdown-menu dropdown-menu-end shadow">
                                            <li><a class="dropdown-item" href="#">Edit category</a></li>
                                            <li><a class="dropdown-item text-danger" href="#">Disable category</a></li>
                                        </ul>
                                    </div>
                                    <button class="btn btn-sm btn-light border">Collapse</button>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>`;
            lastCategoryId = currentCatId;
        }

        // --- SERVİS SATIRI ---
        const external = item.externalServiceInfo || {};
        const providerName = item.provider ? item.provider.name : 'Manual';
        const statusBadge = item.isActive ?
            '<span class="badge bg-success-subtle text-success border border-success-subtle">Enabled</span>' :
            '<span class="badge bg-danger-subtle text-danger border border-danger-subtle">Disabled</span>';

        let icons = "";
        if (item.dripfeed) icons += '<span class="fas fa-tint me-1 text-primary"></span>';
        if (item.cancel) icons += '<span class="fas fa-times-circle me-1 text-danger"></span>';
        if (item.refill) icons += '<span class="far fa-sync-alt me-1 text-warning"></span>';

        htmlOutput += `
            <tr class="service-row ${isAllChecked ? 'table-active' : ''}" data-page="${page}" data-category="${currentCatId}">
                <td class="text-center">
                    <input class="form-check-input service-checkbox" type="checkbox" value="${item.id}" ${isAllChecked ? 'checked' : ''}>
                </td>
                <td class="text-muted">#${item.id}</td>
                <td><div class="fw-bold text-truncate" style="max-width: 250px;" title="${item.name}">${item.name}</div></td>
                <td><div class="small">${item.type || 'Default'}</div>${icons}</td>
                <td><div class="fw-semibold text-truncate" style="max-width: 120px;">${providerName}</div></td>
                <td><div class="fw-bold text-primary">${item.rate}</div></td>
                <td><div>${item.min}</div></td>
                <td><div>${item.max}</div></td>
                <td>${statusBadge}</td>
                <td class="text-end pe-3">
                    <div class="btn-group">
                        <button class="btn btn-sm btn-light border dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                            Actions
                        </button>
                        <ul class="dropdown-menu shadow">
                            <li><a class="dropdown-item" href="javascript:void(0)" onclick="editService(${item.id})">Edit</a></li>
                        </ul>
                    </div>
                </td>
            </tr>`;
    });

    const container = $("#scrollArea");
    const oldHeight = container[0].scrollHeight;

    if (direction === 'append') {
        $("#serviceTable tbody").append(htmlOutput);
        displayedPages.push(page);
    } else {
        $("#serviceTable tbody").prepend(htmlOutput);
        displayedPages.unshift(page);
        container.scrollTop(container.scrollTop() + (container[0].scrollHeight - oldHeight));
    }

    // Temizlik ve Dropdown işlemleri
    if (displayedPages.length > maxPagesInDom) {
        const pageToRemove = (direction === 'append') ? displayedPages.shift() : displayedPages.pop();
        $(`tr[data-page="${pageToRemove}"]`).remove();
    }
    displayedPages.sort((a, b) => a - b);
    const dropdowns = document.querySelectorAll('.dropdown-toggle');
    dropdowns.forEach(el => bootstrap.Dropdown.getOrCreateInstance(el));
}


let draggedRow = null;

$(document).ready(function () {
    initDragAndDrop();
});

function initDragAndDrop() {
    $(document).on('mouseenter', '.service-row, .category-header-row', function () {
        $(this).attr('draggable', true);
    });

    $(document).on('dragstart', '.service-row, .category-header-row', function (e) {
        draggedRow = this;
        $(this).addClass('dragging');
        e.originalEvent.dataTransfer.effectAllowed = 'move';
    });

    $(document).on('dragend', '.service-row, .category-header-row', function () {
        $(this).removeClass('dragging');
        $('.drag-over').removeClass('drag-over');
        draggedRow = null;
    });

    $(document).on('dragover', '.service-row, .category-header-row', function (e) {
        e.preventDefault();
        e.originalEvent.dataTransfer.dropEffect = 'move';

        if (this !== draggedRow) {
            $('.drag-over').removeClass('drag-over');
            $(this).addClass('drag-over');
        }
    });

    $(document).on('dragleave', '.service-row, .category-header-row', function () {
        $(this).removeClass('drag-over');
    });

    $(document).on('drop', '.service-row, .category-header-row', function (e) {
        e.preventDefault();
        $(this).removeClass('drag-over');

        if (!draggedRow || draggedRow === this) return;

        const tbody = $('#serviceTable tbody');
        const target = this;

        const targetOffset = $(target).offset().top;
        const mouseY = e.originalEvent.pageY;

        if (mouseY < targetOffset + ($(target).outerHeight() / 2)) {
            $(draggedRow).insertBefore(target);
        } else {
            $(draggedRow).insertAfter(target);
        }

        saveNewOrder();
    });
}

function saveNewOrder() {
    const orderedIds = [];

    $('.service-row').each(function () {
        const checkbox = $(this).find('.service-checkbox');
        if (checkbox.length) {
            orderedIds.push(checkbox.val());
        }
    });

    console.log('Yeni sıra:', orderedIds);


}  