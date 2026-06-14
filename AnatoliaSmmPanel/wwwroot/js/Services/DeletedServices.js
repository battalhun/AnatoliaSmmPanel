/* =========================
   License Key for AG Grid
========================= */
agGrid.LicenseManager.setLicenseKey('[TRIAL]_this_{AG_Charts_and_AG_Grid}_Enterprise_key_{AG-128628}_is_granted_for_evaluation_only___Use_in_production_is_not_permitted___Please_report_misuse_to_legal@ag-grid.com___For_help_with_purchasing_a_production_key_please_contact_info@ag-grid.com___You_are_granted_a_{Single_Application}_Developer_License_for_one_application_only___All_Front-End_JavaScript_developers_working_on_the_application_would_need_to_be_licensed___This_key_will_deactivate_on_{10 June 2026}____[v3]_[0102]_MTc4MTA0NjAwMDAwMA==27bf590b40f80de6d2c08a74090a07f7');



/* =========================
   Helpers
========================= */


function statusBadge(val) {
    if (val === undefined || val === null) return '';

    return val
        ? '<span class="badge bg-success-subtle text-success border border-success-subtle">Enabled</span>'
        : '<span class="badge bg-danger-subtle text-danger border border-danger-subtle">Disabled</span>';
}

function iconFlags(data) {
    if (!data) return '';

    let icons = '';

    if (data.dripfeed) icons += '<i class="fas fa-tint me-1 text-info" title="Dripfeed"></i>';
    if (data.cancel) icons += '<i class="fas fa-times-circle me-1 text-danger" title="Cancel"></i>';
    if (data.refill) icons += '<i class="fas fa-sync-alt me-1 text-success" title="Refill"></i>';

    return icons;
}

/* =========================
   Columns
========================= */

const columnDefs = [
    {
        width: 50,
        resizable: false,
        sortable: false,
        filter: false,
        pinned: 'left',
        headerCheckboxSelection: true,
        checkboxSelection: p => !!p.data,
        cellRenderer: () => ''
    },
    {
        field: 'id',
        headerName: 'ID',
        width: 90,
        cellRenderer: p => p.data ? `#${p.data.id}` : ''
    },
    {
        field: 'name',
        headerName: 'Service Name',
        flex: 2,
        minWidth: 220,
        cellRenderer: p =>
            p.data ? `<div class="fw-bold text-truncate">${p.data.name}</div>` : ''
    },
    {
        field: 'type',
        headerName: 'Type',
        width: 160,
        cellRenderer: p =>
            p.data
                ? `<div class="small">${p.data.type || 'Default'} ${iconFlags(p.data)}</div>`
                : ''
    },
    {
        headerName: 'Provider',
        width: 150,
        valueGetter: p => p.data?.provider?.name || 'Manual'
    },
    {
        field: 'rate',
        headerName: 'Rate',
        width: 110,
        cellRenderer: p =>
            p.data ? `<span class="fw-bold text-primary">$${parseFloat(p.data.rate).toFixed(4)}</span>` : ''
    },
    {
        field: 'min',
        headerName: 'Min',
        width: 90
    },
    {
        field: 'max',
        headerName: 'Max',
        width: 90
    },
    {
        field: 'isActive',
        headerName: 'Status',
        width: 120,
        cellRenderer: p => p.data ? statusBadge(p.value) : ''
    },
    {
        headerName: 'Actions',
        width: 110,
        pinned: 'right',
        sortable: false,
        filter: false,
        resizable: false,
        cellStyle: { overflow: 'visible' },
        cellRenderer: p => {
            if (!p.data) return '';

            return `
                <div class="dropdown">
                    <button class="btn btn-sm btn-light border dropdown-toggle" data-bs-toggle="dropdown">
                        Actions
                    </button>
                    <ul class="dropdown-menu dropdown-menu-end shadow">
                        <li>
                            <a class="dropdown-item" href="javascript:void(0)" onclick="editService(${p.data.id})">
                                <i class="fas fa-edit me-2 text-primary"></i>Edit
                            </a>
                        </li>
                        <li><hr class="dropdown-divider"></li>
                        <li>
    ${p.data.isActive
                    ? `<a class="dropdown-item text-danger" href="javascript:void(0)" onclick="disableService(${p.data.id})">
               <i class="fas fa-ban me-2"></i>Disable
           </a>`
                    : `<a class="dropdown-item text-success" href="javascript:void(0)" onclick="enableService(${p.data.id})">
               <i class="fas fa-check-circle me-2"></i>Enable
           </a>`
                }
</li>
                    </ul>
                </div>`;
        }
    },
    {
        headerName: '_category',
        rowGroup: true,
        hide: true,
        valueGetter: p => p.data?.serviceCategory?.name || 'Uncategorized'
    }
];

/* =========================
   Grid Options
========================= */

const gridOptions = {
    columnDefs,
    rowData: [],

    groupDisplayType: 'groupRows',
    groupDefaultExpanded: -1,
    rememberGroupStateWhenNewData: true,
    groupSelectsChildren: true,

    onRowClicked: (params) => {
        if (params.node.group) {
            params.node.setExpanded(!params.node.expanded);
        }
    },

    rowSelection: 'multiple',
    suppressRowClickSelection: true,

    groupRowRendererParams: {
        innerRenderer: p => `
            <div class="d-flex align-items-center gap-2">
                <i class="fas fa-folder text-warning"></i>
                <span class="fw-bold">${p.value}</span>
            </div>`
    },

    defaultColDef: {
        sortable: true,
        filter: true,
        resizable: true,
        suppressMenu: true
    },

    onSelectionChanged
};

/* =========================
   Init Grid
========================= */

const gridDiv = document.getElementById('myGrid');
const grid = agGrid.createGrid(gridDiv, gridOptions);

/* =========================
   Selection / Bulk Bar
========================= */

function onSelectionChanged() {
    const count = grid.getSelectedRows().length;
    const bar = document.getElementById('bulkActions');

    if (count > 0) {
        document.getElementById('selectedInfo').textContent = count + ' selected';
        bar.classList.remove('d-none');
        bar.classList.add('d-flex');
    } else {
        bar.classList.add('d-none');
        bar.classList.remove('d-flex');
    }
}

/* =========================
   Bulk Actions (real backend)
========================= */

async function bulkSetActive(isActive) {
    const selected = grid.getSelectedRows();
    if (!selected.length) return;

    const action = isActive ? 'enable' : 'disable';
    if (!confirm(`${action.charAt(0).toUpperCase() + action.slice(1)} ${selected.length} service(s)?`)) return;

    const ids = selected.map(r => r.id);

    try {
        const token = document.querySelector('meta[name="csrf-token"]')?.getAttribute('content') ?? '';

        const res = await fetch('/admin/services/BulkSetActive', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token
            },
            body: JSON.stringify({ ids, isActive })
        });

        const result = await res.json();

        if (result.success) {
            loadData();
        } else {
            alert(result.message || 'Operation failed.');
        }
    } catch (err) {
        alert('Connection error.');
        console.error(err);
    }
}

/* =========================
   Search
========================= */

function onSearch(val) {
    const field = document.getElementById('searchField').value;

    if (field === 'all') {
        grid.setGridOption('quickFilterText', val);
        grid.setFilterModel(null);
    } else {
        grid.setGridOption('quickFilterText', '');
        grid.setFilterModel({
            [field]: {
                filterType: 'text',
                type: 'contains',
                filter: val
            }
        });
    }
}

/* =========================
   Row Actions (modals defined in editservicemodal.js)
========================= */

async function disableService(id) {
    if (!confirm('Are you sure you want to disable this service?')) return;

    try {
        const token = document.querySelector('meta[name="csrf-token"]')?.getAttribute('content') ?? '';

        const res = await fetch(`/admin/services/GetDisableService/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token
            }
        });

        const result = await res.json();

        if (result.success) {
            loadData();
        } else {
            alert(result.message || 'Failed to disable service.');
        }
    } catch (err) {
        alert('Connection error.');
        console.error(err);
    }
}




/* =========================
   Load Data
========================= */

async function loadData() {
    document.getElementById('loadingSpinner').classList.remove('d-none');

    try {
        const res = await fetch('/admin/services/GetReduxAll');
        const json = await res.json();

        if (json.success) {
            grid.setGridOption('rowData', json.data);
            document.getElementById('totalCount').textContent = 'Total: ' + json.total;
        }
    } catch (err) {
        console.error(err);
    } finally {
        document.getElementById('loadingSpinner').classList.add('d-none');
    }
}

loadData();

/* =========================
   Bootstrap dropdown fix after grid renders
========================= */

grid.addEventListener('firstDataRendered', () => {
    document.querySelectorAll('[data-bs-toggle="dropdown"]').forEach(el => {
        bootstrap.Dropdown.getOrCreateInstance(el);
    });
});

/* Reload after Edit/Add Category modal closes successfully */
window.reloadServicesGrid = loadData;