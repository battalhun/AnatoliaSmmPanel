/* =========================
   COOKIE HELPERS (MODERN)
========================= */
const Cookie = {
    set(name, value, days = 365) {
        const d = new Date();
        d.setTime(d.getTime() + days * 864e5);
        document.cookie = `${name}=${value};expires=${d.toUTCString()};path=/`;
    },
    get(name) {
        const key = name + "=";
        return document.cookie
            .split("; ")
            .find(row => row.startsWith(key))
            ?.split("=")[1] || "";
    }
};

/* =========================
   NEW ORDER TAB BUTTON
========================= */
document.querySelectorAll(".btn-new").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelector(".gnyNeworder")?.classList.remove("hidden");

        document.querySelectorAll("ul.nav li a.active")
            .forEach(a => a.classList.remove("active"));

        btn.classList.add("active");
    });
});

/* =========================
   BOOTSTRAP 5 TABS FIX
========================= */
document.querySelectorAll('[data-toggle="tab"]').forEach(el => {
    el.addEventListener("click", () => {
        const target = el.getAttribute("data-target");
        if (!target) return;

        const tabEl = document.querySelector(target);
        if (!tabEl) return;

        bootstrap.Tab.getOrCreateInstance(tabEl).show();
    });
});

/* =========================
   FAVORITE BUTTON (HEADER)
========================= */
document.querySelectorAll(".btn-fav").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll("ul.nav li a.active")
            .forEach(a => a.classList.remove("active"));

        btn.classList.add("active");
        document.querySelector(".gnyNeworder")?.classList.remove("hidden");

        const services = window.modules?.siteOrder?.services || {};
        let count = 0;

        Object.values(services).forEach(s => {
            if (Cookie.get("favorite_service_" + s.id)) count++;
        });
    });
});

/* =========================
   SERVICE RATE
========================= */
function getServiceRate(serviceId) {
    const row = document.querySelector(`tr[data-service-id="${serviceId}"]`);
    if (!row) return console.error("Servis bulunamadı.");

    const cell = row.querySelector(".service-rate");
    if (!cell) return console.error("Rate bulunamadı.");

    return cell.textContent.trim();
}

console.log(getServiceRate(426));

/* =========================
   SERVICE DETAILS
========================= */
function fetchServiceDetails(serviceId) {
    window.onClickService({
        id: serviceId,
        callback: (data) => {
            console.log(`Rate: ${data.rate}`);
        }
    });
}

fetchServiceDetails(426);

/* =========================
   SCROLL BUTTONS
========================= */
document.addEventListener("DOMContentLoaded", () => {
    const prev = document.getElementById("prevButton");
    const next = document.getElementById("nextButton");
    const container = document.getElementById("scrollContainer");

    if (!prev || !next || !container) return;

    const getStep = () => window.innerWidth < 1400 ? 969 : 1185;
    let step = getStep();

    prev.addEventListener("click", () => {
        container.scrollBy({ left: -step, behavior: "smooth" });
    });

    next.addEventListener("click", () => {
        container.scrollBy({ left: step, behavior: "smooth" });
    });

    window.addEventListener("resize", () => step = getStep());
});

/* =========================
   CATEGORY ACTIVE BUTTONS
========================= */
document.querySelectorAll(".nwo-cat-btn").forEach(btn => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".nwo-cat-btn.active")
            .forEach(b => b.classList.remove("active"));

        btn.classList.add("active");
    });
});

/* =========================
   FAVORITE SERVICE TOGGLE
========================= */
document.querySelectorAll(".btn-favorite").forEach(btn => {
    btn.addEventListener("click", () => {
        const id = btn.dataset.serviceId;
        btn.classList.toggle("active");

        if (btn.classList.contains("active")) {
            Cookie.set("favorite_service_" + id, id, 365);
        } else {
            Cookie.set("favorite_service_" + id, "", -1);
        }
    });
});

/* =========================
   INIT FAVORITES STATE
========================= */
document.querySelectorAll(".g-sitem").forEach(item => {
    const id = item.dataset.serviceId;
    const favBtn = item.querySelector(".btn-favorite");

    if (Cookie.get("favorite_service_" + id)) {
        favBtn?.classList.add("active");
        item.dataset.fav = "true";
    } else {
        item.dataset.fav = "false";
    }
});

/* =========================
   FILTER SERVICES
========================= */
const filterInput = document.getElementById("filterServicesInput");

if (filterInput) {
    const nothingFound = document.querySelector(".nothing-found");
    const searchText = document.getElementById("search-text-write");

    filterInput.addEventListener("keyup", e => {
        const keyword = e.target.value.toLowerCase();

        document.querySelectorAll(".g-sitem").forEach(item => {
            const text = item.textContent.toLowerCase();

            if (text.includes(keyword)) {
                item.classList.remove("hidden");
            } else {
                item.classList.add("hidden");
            }
        });

        const cats = document.querySelectorAll(".g-category");
        let emptyCount = 0;

        cats.forEach(cat => {
            const visible = [...cat.querySelectorAll(".g-sitem")]
                .filter(i => !i.classList.contains("hidden"));

            if (!visible.length) {
                cat.style.display = "none";
                emptyCount++;
            } else {
                cat.style.display = "";
            }
        });

        if (emptyCount === cats.length) {
            nothingFound.style.display = "block";
            searchText.textContent = keyword;
        } else {
            nothingFound.style.display = "none";
            searchText.textContent = "";
        }
    });
}

/* =========================
   NO AUTH MENU
========================= */
function noAuthMenu() {
    document.querySelector(".b-menu-wrapper")?.classList.toggle("active");
    document.body.classList.toggle("stop-body");
}

/* =========================
   MODAL SYSTEM
========================= */
function modalOpen(id, data = null) {
    const modal = document.getElementById(id);
    if (!modal) return;

    const box = modal.querySelector(".modal-box");

    modal.classList.add("active");
    document.body.style.overflow = "hidden";

    const close = () => {
        modal.classList.remove("active");
        document.body.style.overflow = "";
    };

    modal.addEventListener("click", e => {
        if (!box.contains(e.target)) close();
    });

    modal.querySelector(".m-close")?.addEventListener("click", close);

    if (data) {
        Object.entries(data).forEach(([k, v]) => {
            const el = document.getElementById(k);
            if (el) el.innerHTML = v;
        });
    }
}

/* =========================
   DASH MENU TOGGLE
========================= */


/* =========================
   GENDER SYSTEM
========================= */
if (!localStorage.gender) localStorage.gender = "male";

const switchEl = document.querySelector(".g-switch");

function genderControl() {
    const avatars = document.querySelectorAll(".user-avatar");

    const female = localStorage.gender === "female";

    avatars.forEach(el => {
        el.src = female
            ? "https://storage.perfectcdn.com/l1wcw9/n8hpwchzzwg5ujya.svg"
            : "https://storage.perfectcdn.com/l1wcw9/yqpw9y5koj5724dn.svg";
    });

    if (switchEl) {
        switchEl.classList.toggle("gender-female", female);
        switchEl.classList.toggle("gender-male", !female);
    }
}

switchEl?.addEventListener("click", () => {
    localStorage.gender =
        localStorage.gender === "female" ? "male" : "female";

    genderControl();
});

genderControl();

/* =========================
   CLIPBOARD
========================= */
async function copyToClipboard(text) {
    try {
        await navigator.clipboard.writeText(text);
        makeToast("Copied");
    } catch {
        const ta = document.createElement("textarea");
        ta.value = text;
        document.body.appendChild(ta);
        ta.select();
        document.execCommand("copy");
        ta.remove();
        makeToast("Copied");
    }
}

/* =========================
   TOAST
========================= */
let toastTimer;

function makeToast(text, time = 4000) {
    const el = document.querySelector(".toast-text");
    const toast = document.querySelector(".bs-toast");

    if (!el || !toast) return;

    el.textContent = text;
    toast.style.display = "block";

    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => {
        toast.style.display = "none";
    }, time);
}

function removeToast() {
    document.querySelector(".bs-toast")?.style && (
        document.querySelector(".bs-toast").style.display = "none"
    );
    clearTimeout(toastTimer);
}

/* =========================
   HEADER SCROLL
========================= */
const headerScroll = () => {
    const header = document.querySelector("#header");
    if (!header) return;

    header.classList.toggle("fixed", window.scrollY > 10);
};

document.addEventListener("scroll", headerScroll);
document.addEventListener("DOMContentLoaded", headerScroll);

/* =========================
   AMOUNT SET
========================= */
function setAmount(val) {
    const input = document.getElementById("amount");
    if (input) input.value = val;
}