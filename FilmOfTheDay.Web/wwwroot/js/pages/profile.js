// wwwroot/js/pages/profile.js

document.addEventListener('DOMContentLoaded', () => {
    // =========================
    // TAB SWITCHING LOGIC
    // =========================
    function showTab(tabName) {
        const tabButtons = document.querySelectorAll('.tab-btn');
        const tabContents = document.querySelectorAll('.tab-content');

        // Hide all tab contents
        tabContents.forEach(tab => tab.classList.add('hidden'));

        // Update tab button colors
        tabButtons.forEach(btn => {
            btn.classList.remove('text-brand-light');
            btn.classList.add('text-brand-muted');
        });

        // Activate clicked tab
        const activeButton = Array.from(tabButtons).find(btn =>
            btn.getAttribute('onclick')?.includes(tabName)
        );
        if (activeButton) {
            activeButton.classList.remove('text-brand-muted');
            activeButton.classList.add('text-brand-light');
        }

        // Show the selected tab
        const activeTab = document.getElementById(tabName);
        if (activeTab) activeTab.classList.remove('hidden');
    }

    // Expose globally for inline onclick()
    window.showTab = showTab;

    // =========================
    // SEND REQUEST FORM
    // =========================
    const sendForm = document.querySelector('.send-request-form');
    if (sendForm) {
        sendForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const btn = sendForm.querySelector('button');
            btn.disabled = true;
            btn.textContent = 'Sending...';
            const formData = new FormData(sendForm);

            try {
                const res = await fetch('/Connection/SendRequest', {
                    method: 'POST',
                    body: formData,
                    credentials: 'same-origin',
                    headers: { 'X-Requested-With': 'XMLHttpRequest' }
                });
                btn.textContent = res.ok ? 'Requested' : 'Error';
            } catch (err) {
                console.error(err);
                btn.textContent = 'Try Again';
                btn.disabled = false;
            }
        });
    }

    // =========================
    // ACCEPT REQUEST FORM
    // =========================
    const acceptForm = document.querySelector('.accept-request-form');
    if (acceptForm) {
        acceptForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const btn = acceptForm.querySelector('button');
            btn.disabled = true;
            btn.textContent = 'Accepting...';
            const formData = new FormData(acceptForm);

            try {
                const res = await fetch('/Connection/AcceptRequest', {
                    method: 'POST',
                    body: formData,
                    credentials: 'same-origin',
                    headers: { 'X-Requested-With': 'XMLHttpRequest' }
                });
                btn.textContent = res.ok ? 'Friends' : 'Error';
                if (res.ok) setTimeout(() => window.location.reload(), 800);
            } catch (err) {
                console.error(err);
                btn.textContent = 'Try Again';
                btn.disabled = false;
            }
        });
    }
});
